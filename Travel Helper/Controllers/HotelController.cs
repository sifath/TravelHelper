using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
using Travel_Helper.Models;

namespace Travel_Helper.Controllers
{
    public class HotelController : Controller
    {
        TMSEntities TMSContext = new TMSEntities();
        // GET: Hotel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BookNow(int id)
        {
            List<hoteldetailinfo> infoRoom = (from i in TMSContext.HotelRoomInfos
                                              where i.HotelId == id
                                              join j in TMSContext.Hotels on id equals j.ID
                                              select new hoteldetailinfo
                                              {
                                                  rID = i.ID,
                                                  Hotelname = j.Name,
                                                  type = i.RoomType,
                                                  rent = i.Rent,
                                                  status = i.Status
                                              }).ToList();


            ViewBag.roomdetails = infoRoom;
            return View();
        }

        public ActionResult Booking(int id)
        {
            if (Session["userid"] == null)
            {
                return View("~/Views/User/Login.cshtml");
            }
            else
            {
                DateTime dt1 = Convert.ToDateTime(Session["cindate"]);
                DateTime dt2 = Convert.ToDateTime(Session["coutdate"]);
                TimeSpan day = dt2.Subtract(dt1);
                var days = day.TotalDays;

                hotelBookingInfo hbf = new hotelBookingInfo();


                for (double i = 0; i < days; i++)
                {

                    hbf.CustomerId = Convert.ToInt32(Session["userid"]);
                    hbf.BookingDate = dt1.AddDays(1);
                    hbf.status = 1;
                    hbf.RoomId = id;

                }

                TMSContext.hotelBookingInfos.Add(hbf);
                TMSContext.SaveChanges();
            }
            Session["cindate"] = null;
            Session["coutdate"] = null;
            return View();
        }
    }
}