using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Travel_Helper.Dataaccess;
using System.Collections;
using System.Collections.Specialized;

namespace Travel_Helper.Controllers
{
    public class UserController : Controller
    {
        //TMSEntities tmsContext = new TMSEntities();
       
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            /*var user = from u in tmsContext.Users
                       where u.Phone == username || u.Email == username
                       select u;*/

            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    User user = (User) context.Users.Single(
                        x => (x.Email.Equals(username.Trim()) || x.Phone.Equals(username.Trim())) && x.Password == password.Trim());

                    

                   
                    if (user.AccessLevel == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Inactive");
                    }


                    if (ModelState.IsValid)
                    {
                       

                        if (user.AccessLevel == 2)  //Bus Owners/admin
                        {
                            Session["busCompanyId"] = user.BusCompanies.First().ID;
                            Session["busName"] = user.BusCompanies.First().Name;
                        }
                        
                        if (user.AccessLevel == 3) //Hotel owners/admin
                            {
                                Session["hotelId"] = user.Hotels.First().ID;
                            }


                        Session["userid"] = user.UID;
                        Session["accessLevel"] = user.AccessLevel;
                        Session["userName"] = user.Name;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception();
                    }

                   
                }
                catch (Exception ex) {
                    ModelState.AddModelError(string.Empty, "Invalid Credentials");

                    
                }
            }
            return View();
            
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            using (TMSEntities tmsContext = new TMSEntities())
            {
                if (tmsContext.Users.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "already in use");
                }

                if (tmsContext.Users.Any(x => x.Phone == user.Phone))
                {
                    ModelState.AddModelError("Phone", "already in use");
                }

                if (ModelState.IsValid)
                {
                    user.AccessLevel = 1;
                    tmsContext.Users.Add(user);
                    tmsContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(user);
        }


        public ActionResult userDashBoard()
        {
            ArrayList busbookings = new ArrayList();
            ArrayList hotelbookings = new ArrayList();
            int userid=Convert.ToInt32(Session["userid"]);
                using (TMSEntities context = new TMSEntities()) { 
                    try{
                    var data = context.SeatBookingInfos.Where(x=>x.customerId==userid);
                        foreach(SeatBookingInfo s in data){
                            busbookings.Add(s);
                        }
                    }catch{
                    }


                    try
                    {
                        var data = context.hotelBookingInfos.Where(x => x.CustomerId == userid);
                        foreach (hotelBookingInfo s in data)
                        {
                            hotelbookings.Add(s);
                        }
                    }
                    catch { }
                }

                ViewData["bus"] = busbookings;
                ViewData["hotel"] = hotelbookings;
           
            return View();
        }


    }

}