using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel_Helper.Models
{
    public class HotelInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int status { get; set;}
        public string Description { get; set; }
        //public string RoomName { get; set; }
    }
}