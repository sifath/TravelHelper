using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
using Travel_Helper.Models;

namespace Travel_Helper.Controllers
{
    public class SearchController : Controller
    {
        TMSEntities context = new TMSEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public ActionResult SearchBus()
        {
            ViewBag.depertureLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
            ViewBag.destinationLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
            return View();
        }




        [HttpPost]
        public ActionResult SearchBus(string depertureLocations,string destinationLocations, string date)
        {
            if(depertureLocations=="")
                ModelState.AddModelError(string.Empty, "Please Select A Departure Location");
            if (destinationLocations == "")
                ModelState.AddModelError(string.Empty, "Please Select A Destination Location");

            try
            {
                if (Convert.ToDateTime(date) == DateTime.MinValue)
                    ModelState.AddModelError(string.Empty, "Please Select a valid Date");
            }
            catch(FormatException e)
            {
                ModelState.AddModelError(string.Empty, "Please Select a valid Date");
            }

            

            if (ModelState.IsValid)
            {
                if (depertureLocations.Equals(destinationLocations))
                    ModelState.AddModelError(string.Empty, "Departure and Destination Location Cann't be Same");

                if(ModelState.IsValid)
                {
                    TempData["DepartureLocation"] = depertureLocations.ToString();
                    TempData["DestinationLocation"] = destinationLocations.ToString();
                    TempData["Date"] = date.Trim();
                    return RedirectToAction("searchResultBus");
                }
                else
                {
                    ViewBag.depertureLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                    ViewBag.destinationLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                    return View();
                }
            }
            else
            {
                ViewBag.depertureLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                ViewBag.destinationLocations = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                return View();
            }
            
        }





        public ActionResult searchResultBus()
        {
            string depertureLocations=(string)TempData["DepartureLocation"] ;
            string destinationLocations = (string)TempData["DestinationLocation"];
            //ViewBag.depertureLocations = depertureLocations;
            //ViewBag.destinationLocations = destinationLocations;
            DateTime date =Convert.ToDateTime(TempData["Date"]);
            ViewBag.date = date.ToLongDateString();

            
                    List<Bus> bs = new List<Bus>();
                    foreach(Bus b in context.Buses)
                    {
                        if(b.DepPlace.ToString().Trim().Equals(depertureLocations.Trim()) && 
                            b.DestPlace.ToString().Trim().Equals(destinationLocations.Trim())&&
                            b.Time.Date== date.Date&&
                            b.Status == 1) 
                        {
                            bs.Add(b);
                        }
                //ViewBag.formate2 = b.Time.Date.ToLongDateString();

            }
                    


                ViewBag.bus = bs;
                return View();

        }





        [HttpGet]
        public ActionResult SearchHotel()
        {
            ViewBag.Location = new SelectList(context.Locations, "ID", "LocationName");
            ViewBag.rType = new SelectList(context.HotelRoomInfos, "ID", "RoomType");
            return View();

        }

        [HttpPost]
        public ActionResult SearchHotel(FormCollection frmCollection)
        {

            string DDValue = Request.Form["LocationValue"].ToString();
            int HotelNumber = Convert.ToInt32(DDValue);

            foreach (var u in context.Locations)
            {
                if (u.ID == HotelNumber)
                    ViewBag.location = u.LocationName;
            }

            string checkInDate = Request.Form["CheckIn"].ToString().Trim();
            DateTime cinDate = Convert.ToDateTime(checkInDate);
            Session["cindate"] = cinDate;
            string checkOutDate = Request.Form["CheckOut"].ToString().Trim();
            DateTime coutDate = Convert.ToDateTime(checkOutDate);
            Session["coutdate"] = coutDate;

            HotelInfo hI = new HotelInfo();

            List<HotelInfo> query = (from hotel in context.Hotels
                         where hotel.AreaId == HotelNumber
                         join hotelRoom in context.HotelRoomInfos on hotel.ID equals hotelRoom.HotelId
                         join bookings in context.hotelBookingInfos on hotelRoom.ID equals bookings.RoomId where (cinDate != bookings.BookingDate || coutDate != bookings.BookingDate)
                         select new HotelInfo
                         {
                             ID = hotel.ID,
                             Name = hotel.Name,
                             //RoomName = hotelRoom.RID,
                             Phone = hotel.Phone,
                             Address = hotel.Address,
                             status = hotelRoom.Status,
                             Description = hotelRoom.Description
                         }).ToList();


            ViewBag.otels = query;
            
            return View("~/Views/Search/SearchHotelPost.cshtml");
        }




    }
}