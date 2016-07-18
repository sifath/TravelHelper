using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Travel_Helper.Dataaccess;
namespace Travel_Helper.Models
{
    public class BusModel
    {
        public Bus BusEntity = new Bus();
        public BusSeatInfo seat = new BusSeatInfo();
        public ArrayList seats = new ArrayList();
        public string Deprature;
        public string Destination;
        public BusModel() { }
        public BusModel(int id) {
            using (TMSEntities context = new TMSEntities()) {
                try
                {
                    this.BusEntity = context.Buses.Single(x => x.ID == id);
                    this.Deprature = BusEntity.Location.LocationName;
                    this.Deprature = BusEntity.Location1.LocationName;
                    this.getSeats();
                }
                catch (Exception ex) {
                    return;
                }
            }
        }

        public bool create(int noOfSeats)
        {
            using (TMSEntities context = new TMSEntities())
            {
                try
                {



                    context.Buses.Add(this.BusEntity);



                    context.SaveChanges();
                }

                catch (Exception e)
                {
                    return false;
                }
                if (!this.createSeats(noOfSeats, this.BusEntity.ID))
                {
                    return false;
                }
            }
            return true;
        }

        private bool createSeats(int noOfSeats, int BusId)
        {
            char[] seatcodes = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int rows = (int)noOfSeats / 4;
            bool last_row_5_seats = noOfSeats % 4 == 0 ? false : true;
            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            BusSeatInfo seat = new BusSeatInfo();
                            seat.BusID = BusId;
                            seat.SID = "" + seatcodes[i] + j;
                            context.BusSeatInfos.Add(seat);

                        }
                    }

                    context.SaveChanges();

                }

                catch (Exception e)
                {
                    return false;
                }
            }


            return true;
        }


        public void getSeats()
        {
            using (TMSEntities context = new TMSEntities())
            {
                try
                {



                    var seats = context.BusSeatInfos.Where(x => x.BusID == this.BusEntity.ID);

                    foreach (BusSeatInfo seat in seats)
                    {
                        this.seats.Add(seat);
                    }
                }

                catch (Exception e)
                {
                    return;
                }

            }

        }
            public ArrayList getTrips(DateTime start){
                ArrayList Buses = new ArrayList();
                try{
                
                using (TMSEntities context = new TMSEntities()) {
                    int cid=Convert.ToInt32(HttpContext.Current.Session["busCompanyId"]);
                    var b = context.Buses.Where(x => x.CompanyId==cid && x.Time.CompareTo(start)>=0);

                    foreach (Bus bus in b) {
                        NameValueCollection bussummary = new NameValueCollection();
                        bussummary["id"] = bus.ID.ToString();
                        bussummary["bid"] = bus.CID;
                        bussummary["name"] = bus.Name;
                        bussummary["start"] = bus.Time.ToLongDateString();
                        bussummary["from"] = bus.Location.LocationName;
                        bussummary["destination"] = bus.Location1.LocationName;
                        bussummary["totalseats"] = context.BusSeatInfos.Count(x => x.BusID == bus.ID).ToString();
                        bussummary["availalbeseats"] = context.BusSeatInfos.Count(x => x.BusID == bus.ID && x.Status == 0).ToString();

                        Buses.Add(bussummary);
                    }

                    }
                }

                    catch(Exception )
                {

                    }
                return Buses;
            

            }
            public ArrayList getTrips(DateTime start, DateTime end)
            {
                ArrayList Buses = new ArrayList();
                try
                {

                    using (TMSEntities context = new TMSEntities())
                    {
                        int cid = Convert.ToInt32(HttpContext.Current.Session["busCompanyId"]);

                        var b = context.Buses.Where(x => x.CompanyId==cid &&  x.Time >= start && x.Time <= end);

                        foreach (Bus bus in b)
                        {
                            NameValueCollection bussummary = new NameValueCollection();
                            bussummary["id"] = bus.ID.ToString();
                            bussummary["bid"] = bus.CID;
                            bussummary["name"] = bus.Name;
                            bussummary["start"] = bus.Time.ToLongDateString();
                            bussummary["from"] = bus.Location.LocationName;
                            bussummary["destination"] = bus.Location1.LocationName;
                            bussummary["totalseats"] = context.BusSeatInfos.Count(x => x.BusID == bus.ID).ToString();
                            bussummary["availalbeseats"] = context.BusSeatInfos.Count(x => x.BusID == bus.ID && x.Status==0).ToString();

                            Buses.Add(bussummary);
                        }

                    }
                }

                catch (Exception)
                {

                }
                return Buses;


            }
        }
    }

