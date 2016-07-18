using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
using Travel_Helper.Models;

namespace Travel_Helper.Controllers
{
    public class BusdashboardController : Controller
    {
        // GET: Busdashboard
        public ActionResult Index()
        {
            BusModel b = new BusModel();
            accessControl.accessValidation(2, "~/Home/Index");
            DateTime time = DateTime.Today;
            ViewData["Buses"] = b.getTrips(time);
            return View();
        }

        // GET: Busdashboard
        [HttpPost]

        public ActionResult Index(FormCollection collection)
        {
            accessControl.accessValidation(2, "~/Home/Index");
            BusModel b = new BusModel();

            DateTime from = Convert.ToDateTime(collection["fromdate"].Trim());

            DateTime to = Convert.ToDateTime(collection["todate"].Trim());
            ViewData["Buses"] = b.getTrips(from, to);
            return View();
        }

        // GET: Busdashboard/Details/5
        public ActionResult Details(int id)
        {
            accessControl.accessValidation(2, "~/Home/Index", id);

            BusModel bus = new BusModel(id);
            accessControl.accessValidation(2, "~/Home/Index");
            return View(bus);
        }

        // GET: Busdashboard/Create
        public ActionResult Create()
        {
            accessControl.accessValidation(2, "~/Home/Index");

       
            
            using (TMSEntities context = new TMSEntities()) {
                ArrayList locations = new ArrayList();

                var areas = context.Locations.ToList();

                foreach (Location l in areas)
                    locations.Add(l);

                ViewData["areas"]=locations;
            }

            
       

            return View();
        }

        // POST: Busdashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            accessControl.accessValidation(2, "~/Home/Index");
            try
            {
             
                    // TODO: Add insert logic here
                    BusModel bus = new BusModel();
                    bus.BusEntity.Name = collection["form-busname"].Trim();
                    bus.BusEntity.CID = collection["form-busid"].Trim();
                    bus.BusEntity.Description = collection["form-desc"].Trim();
                    bus.BusEntity.SeatRent = Convert.ToDecimal(collection["form-seatrent"].Trim());
                    bus.BusEntity.DepPlace = Convert.ToInt32(collection["deprature"].Trim());
                    bus.BusEntity.DestPlace = Convert.ToInt32(collection["destination"].Trim());
                    bus.BusEntity.CompanyId = Convert.ToInt32(Session["busCompanyId"]);
                   
                    bus.BusEntity.Time = Convert.ToDateTime(collection["form-time"].Trim());

                    if(bus.create(Convert.ToInt32(collection["form-seatcount"]))){
                        return RedirectToAction("Details/" + bus.BusEntity.ID);
                    }

                    return RedirectToAction("Index");
                
            }
            catch
            {

                using (TMSEntities context = new TMSEntities())
                {
                    ArrayList locations = new ArrayList();

                    var areas = context.Locations.ToList();

                    foreach (Location l in areas)
                        locations.Add(l);

                    ViewData["areas"] = locations;
                }

                return View();
            }
        }

        // GET: Busdashboard/BookSeat/{seatid}
        public ActionResult BookSeat(int id)
        {
            accessControl.accessValidation(2, "~/Home/Index");

            int busid=0;
            using (TMSEntities context = new TMSEntities()){
                try
                {
                    BusSeatInfo seat = context.BusSeatInfos.First(x => x.ID == id);

                    seat.Status = 1;
                    busid = seat.BusID;

                    SeatBookingInfo b  = new SeatBookingInfo();
                    b.SeatId = id;  
                    b.customerId = Convert.ToInt32(Session["userid"]);
                    b.Status = 1;

                    context.SeatBookingInfos.Add(b);

                    context.SaveChanges();

                }

                catch (Exception e) {
                    return RedirectToAction("Index");
                }
            }

            if (busid == 0)
                return RedirectToAction("Index");
              return RedirectToAction("Details/" + busid);
           
        }

        // GET: Busdashboard/BookSeat/{seatid}
        public ActionResult CancelSeat(int id)
        {
            accessControl.accessValidation(2, "~/Home/Index");

            int busid = 0;
            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    BusSeatInfo seat = context.BusSeatInfos.First(x => x.ID == id);

                    seat.Status = 0;
                    busid = seat.BusID;

                    SeatBookingInfo b = context.SeatBookingInfos.First(x => x.SeatId == id);

                    context.SeatBookingInfos.Remove(b);

                    context.SaveChanges();
                }

                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }
            }

            if (busid == 0)
                return RedirectToAction("Index");
            return RedirectToAction("Details/" + busid);

        }

        // GET: Busdashboard/Edit/5
        public ActionResult Edit(int id)
        {
            accessControl.accessValidation(2, "~/Home/Index");
            return View();
        }

        // POST: Busdashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            accessControl.accessValidation(2, "~/Home/Index");
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
    }
}
