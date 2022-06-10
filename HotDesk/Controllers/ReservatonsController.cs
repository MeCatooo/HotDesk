using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotDesk.Models;

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
        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            if (reservation.From>DateTime.Now|| reservation.From > DateTime.Now || reservation.From<reservation.To)
            {
                _repository.AddReservation(reservation);
                return Ok(reservation);
            }
            else
                return BadRequest();
        }

        // GET: ReservatonsController/Edit/5
        [HttpPatch("{id}")]
        public ActionResult Edit(int id,int Deskid)
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
    }
}
