using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
using Travel_Helper.Models;

namespace Travel_Helper.Controllers
{

    public class AdminController : Controller
    {
        TMSEntities context = new TMSEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("AdminPanel");
        }


        [HttpGet]
        public ActionResult AdminPanel()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            return View();
        }



        //------------------------------------------
        //*************** Create Bus****************
        //---------------------------------------------
        [HttpGet]
        public ActionResult CreateBusCompany()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            return View(); 
        }

        [HttpPost]
        public ActionResult CreateBusCompany(BusCompany busC)
        {
            if(busC.Name==null)
                ModelState.AddModelError(string.Empty, "Company Name is Required");
            if(busC.Phone==null)
                ModelState.AddModelError(string.Empty, "Company Phone Number is required");
            if(busC.Email==null)
                ModelState.AddModelError(string.Empty, "Email is Required");
            if (busC.Description == null)
                ModelState.AddModelError(string.Empty, "Description is Required");
            if (busC.Address == null)
                ModelState.AddModelError(string.Empty, "Address is Required");

            
            if (ModelState.IsValid)
            {
                Session["busOncreate"] = busC;
                return RedirectToAction("CreateBusAdmin");
            }
            else
                return View(busC);
        }


        [HttpGet]
        public ActionResult CreateBusAdmin()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            return View();
        }

        [HttpPost]
        public ActionResult CreateBusAdmin(User admin)
        {
            BusCompany newBusCompany = (BusCompany)Session["busOncreate"];

            if(admin.Phone==null)
                ModelState.AddModelError(string.Empty, "Phone Number Can not be Null");
            if (admin.Email == null)
                ModelState.AddModelError(string.Empty, "Email Can not be Null");
            if (admin.Name == null)
                ModelState.AddModelError(string.Empty, "Name Can not be Null");
            if (admin.Password == null)
                ModelState.AddModelError(string.Empty, "Please Give a Password");
            if (admin.Address == null)
                ModelState.AddModelError(string.Empty, "Address Can not be Null");
            if (admin.NID == null)
                ModelState.AddModelError(string.Empty, "NID Can not be Null");



            if (ModelState.IsValid)
            {
                var v = from u in context.Users
                        where u.Phone == admin.Phone || u.Email == admin.Email || u.NID == admin.NID
                        select u;


                var user = from u in v
                           where u.Phone == admin.Phone
                           select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "Phone Number is Already Registered With anothr Account");

                user = from u in v
                       where u.Email == admin.Email
                       select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "This Email is Already Registered With anothr Account");

                user = from u in v
                       where u.NID == admin.NID
                       select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "This NID is Already Registered With anothr Account");


                if(ModelState.IsValid)
                {
                    admin.AccessLevel = 2;
                    context.Users.Add(admin);
                    context.SaveChanges();

                    newBusCompany.AdminId = v.First().UID;
                    context.BusCompanies.Add(newBusCompany);
                    context.SaveChanges();
                    return RedirectToAction("succefullyCreated");
                }

            }
            return View();
        }




        //------------------------------------------
        //*************** Create Hotel****************
        //---------------------------------------------

        [HttpGet]
        public ActionResult CreateHotel()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            ViewBag.AreaId= new SelectList(context.Locations.ToList(), "ID", "LocationName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateHotel(Hotel hotel)
        {
            if (hotel.Name == null)
                ModelState.AddModelError(string.Empty, "Hotel Name is Required");
            if (hotel.Phone == null)
                ModelState.AddModelError(string.Empty, "Hotel Phone Number is required");
            if (hotel.Email == null)
                ModelState.AddModelError(string.Empty, "Hotel Email is Required");
            if (hotel.Description == null)
                ModelState.AddModelError(string.Empty, "Hotel Description is Required");
            if (hotel.Address == null)
                ModelState.AddModelError(string.Empty, "Hotel Address is Required");
            if(hotel.AreaId==0)
                ModelState.AddModelError(string.Empty, "Please Select a Location");



            if (ModelState.IsValid)
            {
                Session["hotelOncreate"] = hotel;
                return RedirectToAction("CreateHotelAdmin");
            }
            else
            {
                ViewBag.AreaId = new SelectList(context.Locations.ToList(), "ID", "LocationName");
                return View(hotel);
            }
                
        }


        [HttpGet]
        public ActionResult CreateHotelAdmin()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            return View();
        }

        [HttpPost]
        public ActionResult CreateHotelAdmin(User admin)
        {
            Hotel newHotel = (Hotel)Session["hotelOncreate"];

            if (admin.Phone == null)
                ModelState.AddModelError(string.Empty, "Admin Phone Number is Required");
            if (admin.Email == null)
                ModelState.AddModelError(string.Empty, "Admin Email is Required");
            if (admin.Name == null)
                ModelState.AddModelError(string.Empty, "Admin Name is Required");
            if (admin.Password == null)
                ModelState.AddModelError(string.Empty, "Admin Password is Required");
            if (admin.Address == null)
                ModelState.AddModelError(string.Empty, "Admin Address is Required");
            if (admin.NID == null)
                ModelState.AddModelError(string.Empty, "Admin NID is Required");



            if (ModelState.IsValid)
            {
                var v = from u in context.Users
                        where u.Phone == admin.Phone || u.Email == admin.Email || u.NID == admin.NID
                        select u;


                var user = from u in v
                           where u.Phone == admin.Phone
                           select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "Phone Number is Already Registered With anothr Account");

                user = from u in v
                       where u.Email == admin.Email
                       select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "This Email is Already Registered With anothr Account");

                user = from u in v
                       where u.NID == admin.NID
                       select u;

                if (user.Any())
                    ModelState.AddModelError(string.Empty, "This NID is Already Registered With anothr Account");


                if (ModelState.IsValid)
                {
                    admin.AccessLevel = 3;
                    context.Users.Add(admin);
                    context.SaveChanges();

                    newHotel.AdminId = v.First().UID;
                    context.Hotels.Add(newHotel);
                    context.SaveChanges();
                    return RedirectToAction("succefullyCreated");
                }

            }
            return View();
        }












        public ActionResult succefullyCreated()
        {
            accessControl.accessValidation(9, "~/Home/Index");
            return View();
        }


    }
}