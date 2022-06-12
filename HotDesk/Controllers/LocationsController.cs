using HotDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private IHotDeskRepository _repository;
        public LocationsController(IHotDeskRepository hotDeskRepository)
        {
            _repository = hotDeskRepository;
        }
        //GET: LocationsController
        [HttpGet]
        public ActionResult Index()
        {
            return Ok(_repository.GetAllLocations());
        }
        [HttpGet("{id}")]
        // GET: LocationsController/Details/5
        public ActionResult Details(int id)
        {
            var get = _repository.GetLocation(id);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get);
        }
        [HttpGet("{id}/desks")]
        public ActionResult GetDesks(int id)
        {
            var get = _repository.GetLocation(id);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get.Desks);
        }
        [HttpGet("{id}/occupied")]
        public ActionResult OcupiedDesks(int id, [FromBody] TimeStamps dates)
        {
            var get = ReservationLogic.OccupiedDesks(id, dates.From, dates.To, _repository);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get);
        }
        [HttpGet("{id}/free")]
        public ActionResult FreeDesks(int id, [FromBody] TimeStamps dates)
        {
            var get = ReservationLogic.OccupiedDesks(id, dates.From, dates.To, _repository);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get);
        }

        // POST: LocationsController/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] string name)
        {
            if (String.IsNullOrEmpty(name))
                return BadRequest();
            Location location = new Location() { Name = name };
            location = _repository.AddLocation(location);
            return Ok(location);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("{locationId}/desk")]
        public ActionResult AddDesk(int locationId, string name)
        {
            if (String.IsNullOrEmpty(name))
                return BadRequest();
            var location = _repository.GetLocation(locationId);
            Desk desk = new Desk() { Name = name, Location = location };
            desk = _repository.AddDesk(desk);
            _repository.BindDeskToLocation(desk.Id, locationId);
            return Ok(_repository.GetLocation(locationId));
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        [Route("desk/{deskId}")]
        public ActionResult RemoveDesk(int deskId)
        {
            var desk = _repository.GetDesk(deskId);
            if (_repository.GetAllReservations().Any(a => a.Id == deskId))
                return BadRequest();
            _repository.RemoveDesk(deskId);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPatch]
        [Route("desk/state/{deskId}")]
        public ActionResult ChangeStateDesk(int deskId, [FromBody] bool state)
        {
            var desk = _repository.GetDesk(deskId);
            if (_repository.GetAllReservations().Any(a => a.Id == deskId))
                return BadRequest("First remove all reservations from desk");
            _repository.UpdateDeskAvaibality(deskId, state);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        [Route("{id}")]
        public ActionResult RemoveLocation(int id)
        {
            var location = _repository.GetLocation(id);
            if (location.Desks.Count > 0)
                return BadRequest("First remove all desks from location");
            _repository.RemoveLocation(id);
            return Ok();
        }
    }
}
