using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotDesk.Models;
using JwtApp.Models;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace HotDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservatonsController : Controller
    {
        private IHotDeskRepository _repository;
        public ReservatonsController(IHotDeskRepository hotDeskRepository)
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
            var get = _repository.GetReservation(id);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok();
        }

        // GET: ReservatonsController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ReservatonsController/Create
        [HttpPost("/location/{id}/desk/{deskId}")]
        public ActionResult Create(int id, int deskId)
        {
            DateTime from = new DateTime(2023, 1, 1);
            DateTime to = new DateTime(2023, 1, 2);
            if (from <= DateTime.Now && from <= DateTime.Now && from >= to)
                return BadRequest();
            Desk desk = _repository.GetDesk(deskId);
            Location location = _repository.GetLocation(id);
            if (ReferenceEquals(desk, null) || ReferenceEquals(location, null) || !location.Desks.Any(a=>a.Id==deskId))
                return BadRequest();
            if (!ResrvationLogic.IsDeskReserved(desk, from, to, _repository))
                return BadRequest("Brak wolnego biurka");
            Reservation reservation = _repository.AddReservation(new Reservation()
            {
                user =GetCurrentUser(),
                desk = desk,
                location = location,
                From = from,
                To = to
            });;
            return Ok(reservation);

        }

        // GET: ReservatonsController/Edit/5
        [HttpPatch("{id}")]
        public ActionResult Edit(int id, int Deskid)
        {
            var tmp = _repository.GetReservation(id);
            var newDesk = _repository.GetDesk(Deskid);
            if (ReferenceEquals(tmp, null) || ReferenceEquals(newDesk, null))
                return NotFound();
            //if(tmp.From -) ///todo
            tmp.desk = newDesk;
            _repository.UpdateReservation(tmp.Id, tmp);
            return Ok(tmp);
        }

        // POST: ReservatonsController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ReservatonsController/Delete/5
        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ReservatonsController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return _repository.GetUser(new UserLogin() { Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value });
            }
            return null;
        }
    }
}
