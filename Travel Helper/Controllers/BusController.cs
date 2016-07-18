using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
namespace Travel_Helper.Controllers
{
    
    public class BusController : Controller
    {
        TMSEntities context = new TMSEntities();
        // GET: Bus
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult buyTicket(int id)
        {
            
            ViewBag.busId = id;
            return View();
        }

        [HttpGet]
        public ActionResult buy(int id)
        {
            //context.BusCompanies
            SeatBookingInfo sbi = new SeatBookingInfo();
            sbi.SeatId = id;
            sbi.customerId = (int)Session["userid"];
            sbi.Status = 1;
            context.SeatBookingInfos.Add(sbi);
            context.BusSeatInfos.Find(id).Status = 1;
            context.SaveChanges();
            int busID = context.BusSeatInfos.Find(id).BusID;
            //return RedirectToAction("buyTicket");
            return RedirectToAction("buyTicket", new { id = busID });
        }
    }
}