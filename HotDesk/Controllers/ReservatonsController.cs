using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotDesk.Models;

namespace HotDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservatonsController : Controller
    {
        private MemoryRepository _reservationRepository;
        public ReservatonsController()
        {
            _reservationRepository = new MemoryRepository();
        }
        // GET: ReservatonsController
        [HttpGet]
        public ActionResult Index()
        {
            return Ok(_reservationRepository.GetAll());
        }

        // GET: ReservatonsController/Details/5
        [HttpGet("{id}")]
        public ActionResult Details(int id)
        {
            return Ok();
        }

        // GET: ReservatonsController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ReservatonsController/Create
        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            try
            {
                _reservationRepository.Add(reservation);
                return Ok(reservation);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: ReservatonsController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

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
    }
}
