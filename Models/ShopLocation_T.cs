//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TauseelRestaurant.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShopLocation_T
    {
        public int ShopLocationID { get; set; }
        public Nullable<int> ShopID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<decimal> LocDeliveryCharge { get; set; }
        public string LocDeliveryTime { get; set; }
        public Nullable<decimal> LocMinOrder { get; set; }
    }
}
