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
    public class HoteldashboardController : Controller
    {
        // GET: Hoteldashboard
        public ActionResult Index()
        {
            accessControl.accessValidation(3, "~/Home/Index");

            return View(new HotelModel(Convert.ToInt32(Session["hotelId"])));
        }

        // GET: Hoteldashboard/Details/5
        public ActionResult Details(int id)
        {
            accessControl.accessValidation(3, "~/Home/Index", id);

            HotelModel hotel = new HotelModel(Convert.ToInt32(Session["hotelId"]));
            
            return View(hotel);
        }

        

        // GET: Hoteldashboard/Edit/5
        public ActionResult Edit()
        {
            accessControl.accessValidation(3, "~/Home/Index");

            Hotel model;
            using (TMSEntities context = new TMSEntities()) {
                int hid=Convert.ToInt32(Session["hotelId"]);
                 model =context.Hotels.Single(x=>x.ID==hid);
                ViewBag.AreaId = new SelectList(context.Locations.ToList(), "ID", "LocationName");
            }
            return View(model);
        }

        // POST: Hoteldashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(Hotel collection)
        {
            accessControl.accessValidation(3, "~/Home/Index");

            try
            {
                // TODO: Add update logic here
                using (TMSEntities context = new TMSEntities())
                {
                    Hotel h = context.Hotels.Single(x=>x.ID==Convert.ToInt32(Session["hotelId"]));
                    h = collection;

                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                using (TMSEntities context = new TMSEntities())
                {
                    int hid = Convert.ToInt32(Session["hotelId"]);
                    ViewBag.AreaId = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                }
                return View(collection);
            }
        }
        public ActionResult ViewRoom(int id)
        {
            accessControl.accessValidation(3, "~/Home/Index");

            HotelRoom room = new HotelRoom(id);
            return View(room);
        }
        public ActionResult CreateRoom()
        {

            return View();
        }
       
        // POST: Hoteldashboard/Edit/5
        [HttpPost]
        public ActionResult CreateRoom(HotelRoomInfo collection)
        {
            accessControl.accessValidation(3, "~/Home/Index");

            try
            {
                // TODO: Add update logic here
                using (TMSEntities context = new TMSEntities())
                {
                    collection.HotelId = Convert.ToInt32(Session["hotelId"]);
                    collection.Status = 0;
                   

                    context.HotelRoomInfos.Add(collection);
                    context.SaveChanges();
                    return RedirectToAction("ViewRoom/" + collection.ID);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(collection);
            }
        }

       

  // Hoteldashboard/BookRoom/5
        
        public ActionResult BookRoom(int id)
        {
            accessControl.accessValidation(3, "~/Home/Index");

            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    HotelRoomInfo room = context.HotelRoomInfos.Single(x => x.ID == id);
                    ViewData["room"] = room;
                    return View();
                }
                catch
                {

                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult BookRoom(int id, hotelBookingInfo collection)
        {

            accessControl.accessValidation(3, "~/Home/Index");

            
            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    collection.CustomerId = Convert.ToInt32(Session["userid"]);
                    collection.RoomId = id;
                    context.hotelBookingInfos.Add(collection);
                    context.SaveChanges();
                    return RedirectToAction("ViewRoom/"+id);
                }
                catch
                {

                }
            }

            return RedirectToAction("Index");
        }

        // POST: Hoteldashboard/Edit/5

        public ActionResult CancelBooking(int id)
        {
            accessControl.accessValidation(3, "~/Home/Index");

            hotelBookingInfo booking;
            using (TMSEntities context = new TMSEntities())
            {
                try { 
                 booking = context.hotelBookingInfos.Single(x => x.BID == id);

                context.hotelBookingInfos.Remove(booking);
                return RedirectToAction("ViewRoom/" + booking.RoomId);
                    }
                catch
                {
                    
                }
            }

            return RedirectToAction("Index");
        }
       
    }
}
