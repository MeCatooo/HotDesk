using HotDesk.Models;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        private IHotDeskRepository _repository;
        public ReservationsController(IHotDeskRepository hotDeskRepository)
        {
            _repository = hotDeskRepository;
        }
        // GET: ReservatonsController
        [HttpGet]
        public ActionResult Index()
        {
            return Ok(_repository.GetAllReservations());
        }

        // GET: ReservatonsController/Details/5
        [HttpGet("{id}")]
        public ActionResult Details(int id)
        {
            UserModel user = GetCurrentUser();
            Reservation get;
            if (!ReferenceEquals(user, null) && user.Role == "Administrator")
                get = _repository.GetReservationAdmin(id);
            else
                get = _repository.GetReservation(id);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get);
        }
        [HttpGet("location/{id}")]
        public ActionResult GetReservationsInLocation(int id)
        {
            UserModel user = GetCurrentUser();
            List<Reservation> get = new List<Reservation>();
            if (!ReferenceEquals(user, null) && user.Role == "Administrator")
                get = _repository.GetAllReservationsAdmin().Where(a => a.Location.Id == id).ToList();
            else
                get = _repository.GetAllReservations().Where(a => a.Location.Id == id).ToList();
            return Ok(get);
        }

        // POST: ReservatonsController/Create
        [Authorize]
        [HttpPost("location/{id}/desk/{deskId}")]
        public ActionResult Create(int id, int deskId, [FromBody] TimeStamps data)
        {
            DateTime from = data.From.Date;
            DateTime to = data.To.Date;
            if (from.Subtract(to) > new TimeSpan(168, 0, 0)) //7 * 24 = 168
                return BadRequest("Reservation can't be longer than 7 days");
            if (from <= DateTime.Now && from <= DateTime.Now && from >= to)
                return BadRequest();
            Desk desk = _repository.GetDesk(deskId);
            Location location = _repository.GetLocation(id);
            if (ReferenceEquals(desk, null) || ReferenceEquals(location, null) || !location.Desks.Any(a => a.Id == deskId))
                return BadRequest();
            if (!ReservationLogic.IsDeskReserved(desk, from, to, _repository))
                return BadRequest("No free desk");
            Reservation reservation = _repository.AddReservation(new Reservation()
            {
                User = GetCurrentUser(),
                Desk = desk,
                Location = location,
                From = from,
                To = to
            });
            return Ok(reservation);

        }

        // GET: ReservatonsController/Edit/5
        [HttpPatch("{id}")]
        public ActionResult Edit(int id, int deskId)
        {
            var reservation = _repository.GetReservation(id);
            var newDesk = _repository.GetDesk(deskId);
            if (ReferenceEquals(reservation, null) || ReferenceEquals(newDesk, null) || !reservation.Location.Desks.Any(a => a.Id == deskId))
                return NotFound();
            if (!ReservationLogic.IsDeskReserved(newDesk, reservation.From, reservation.To, _repository))
                return BadRequest("No free desk");
            _repository.UpdateReservationDesk(reservation.Id, newDesk.Id);
            return Ok(reservation);
        }
        [HttpDelete]
        [Route("remove/{id}")]
        public ActionResult Remove(int id)
        {
            _repository.RemoveReservation(id);
            return Ok();

        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity.Claims.Count() != 0)
            {
                var userClaims = identity.Claims;

                return _repository.GetUser(new UserLogin() { Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value });
            }
            return null;
        }

    }
}
