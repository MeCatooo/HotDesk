using HotDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("{id}/ocupied")]
        public ActionResult OcupiedDesks(int id,[FromBody] TimeStamps dates)
        {
            var get = ReservationLogic.OccupiedDesks(id, dates.From, dates.To, _repository);
            if (ReferenceEquals(get, null))
            {
                return NotFound();
            }
            return Ok(get);
        }
        [HttpGet("{id}/free")]
        public ActionResult FreeDesks(int id,[FromBody] TimeStamps dates)
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
        [Route("api/[controller]/{id}/add/desk")]
        public IActionResult AddDesk(int locationId,string name)
        {
            if (String.IsNullOrEmpty(name))
                return BadRequest();
            var location = _repository.GetLocation(locationId);
            Desk desk = new Desk() { Name = name, Location=  location};
            desk = _repository.AddDesk(desk);
            _repository.BindDeskToLocation(desk.Id, locationId);
            return Ok(_repository.GetLocation(locationId));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("api/[controller]/{id}/remove/desk/{deskId}")]
        public IActionResult RemoveDesk(int id,int deskId)
        {
            var location = _repository.GetLocation(id);
            if (!location.Desks.Any(a => a.Id == deskId))
                return BadRequest();
            location.Desks.Remove(location.Desks.First(a => a.Id == deskId));
            return Ok("Delted");
        }
        //// POST: LocationsController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
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

        //// GET: LocationsController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: LocationsController/Edit/5
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

        //// GET: LocationsController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: LocationsController/Delete/5
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
    }
}
