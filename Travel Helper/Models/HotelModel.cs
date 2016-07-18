using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Travel_Helper.Dataaccess;
namespace Travel_Helper.Models
{
    public class HotelModel
    {
        public Hotel HotelEntity = new Hotel();
        public ArrayList Rooms = new ArrayList();
        
        
        public HotelModel() { }
        
        public HotelModel(int id)
        {
            using (TMSEntities context = new TMSEntities()){
                this.HotelEntity= context.Hotels.Single(x=>x.ID==id);
            }
            this.getHotelRooms();
        }
         public void getHotelRooms()
        {
            using (TMSEntities context = new TMSEntities())
            {
                try
                {



                    var rooms = context.HotelRoomInfos.Where(x => x.HotelId == this.HotelEntity.ID);

                    foreach (HotelRoomInfo room in rooms)
                    {
                        HotelRoom r = new HotelRoom();
                        r.Room=room;
                        r.getBookingInfo();
                        Rooms.Add(r);
                    }
                }

                catch (Exception e)
                {
                    return;
                }

            }

        }
    }
}