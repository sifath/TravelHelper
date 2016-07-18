using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel_Helper.Dataaccess;
namespace Travel_Helper.Models
{
    public class HotelRoom
    {
        public HotelRoomInfo Room;
        public ArrayList Bookings = new ArrayList();
        public ArrayList BookingHistory = new ArrayList();
        public HotelRoom() { }

        public HotelRoom(int id) {

            using (TMSEntities context = new TMSEntities()) {
                this.Room = context.HotelRoomInfos.Single(x => x.ID == id);
            }
            this.getBookingInfo();
            this.getBookingHistory();
        }

        public void getBookingInfo() { 
        
            using (TMSEntities context = new TMSEntities()){
                try
                {
                    var b = context.hotelBookingInfos.Where(x => x.BookingDate >= DateTime.Now && x.RoomId == this.Room.ID);

                    foreach (hotelBookingInfo booking in b)
                    {
                        Bookings.Add(booking);
                    }
                }
                catch (Exception e) { }

            }

        }

        public void getBookingHistory()
        {

            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    var b = context.hotelBookingInfos.Where(x =>  x.RoomId == this.Room.ID);

                    foreach (hotelBookingInfo booking in b)
                    {
                        BookingHistory.Add(booking);
                    }
                }
                catch (Exception e) { }

            }

        }

        public User getBookingUser(int id) {
            User u= new User();
            using (TMSEntities context = new TMSEntities())
            {
                try
                {
                    hotelBookingInfo b = context.hotelBookingInfos.Single(x => x.BID == id);

                    u = b.User;
                }
                catch (Exception e) { }

            }
            return u;
        }

        public Boolean getStatus() {
            using (TMSEntities context = new TMSEntities())
            {
                var b = context.hotelBookingInfos.Where(x => x.BookingDate >= DateTime.Now && x.BookingDate<= DateTime.Today.AddDays(1).AddMilliseconds(-1) && x.RoomId == this.Room.ID);
                try
                {
                    if (b.Count() != 0)
                        return true;
                }
                catch
                {
                    
                }
            }
            return false;
        }

        public Hotel getHoltel(int rid) {
            using (TMSEntities context = new TMSEntities()) {
                HotelRoomInfo r = context.HotelRoomInfos.Single(x => x.ID == rid);
                return context.Hotels.Single(x => x.ID == r.HotelId);
            }
        }
    }
}