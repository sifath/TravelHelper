//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Travel_Helper.Dataaccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class SeatBookingInfo
    {
        public int BID { get; set; }
        public int customerId { get; set; }
        public int SeatId { get; set; }
        public byte Status { get; set; }
    
        public virtual BusSeatInfo BusSeatInfo { get; set; }
        public virtual User User { get; set; }
    }
}