using HotDesk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
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

        // POST: LocationsController/Create
        [HttpPost]
        public ActionResult Create()
        {
            return View();
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
