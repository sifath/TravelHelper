using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Travel_Helper.Dataaccess;

namespace Travel_Helper.Models
{
    public class accessControl 
    {
        public static void accessValidation(int accessLevel,string reDirectTo)
        {
            if(HttpContext.Current.Session["userid"]!=null)
            {
                if(!HttpContext.Current.Session["accessLevel"].ToString().Trim().Equals(accessLevel.ToString().Trim()))
                {
                    HttpContext.Current.Response.Redirect(reDirectTo);
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Home/Index");
            }
        }

        public static void accessValidation(int accessLevel, string reDirectTo, int busorhotelid)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!HttpContext.Current.Session["accessLevel"].ToString().Trim().Equals(accessLevel.ToString().Trim()))
                {
                    try
                    {
                        using (TMSEntities context = new TMSEntities())
                        {
                            if (accessLevel == 2)
                            {
                                int entity = context.Buses.Count(x => x.BusCompany.User.UID == Convert.ToInt32(HttpContext.Current.Session["userid"]));

                                if (entity > 1)
                                {
                                    HttpContext.Current.Response.Redirect(reDirectTo);
                                }
                            }

                            if (accessLevel == 3)
                            {
                                int entity = context.Hotels.Count(x => x.User.UID == Convert.ToInt32(HttpContext.Current.Session["userid"]));

                                if (entity > 1)
                                {
                                    HttpContext.Current.Response.Redirect(reDirectTo);
                                }

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        HttpContext.Current.Response.Redirect("~/Home/Index");
                    }

                    
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Home/Index");
            }
        }
    }
}