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
    
    public partial class Shop_T
    {
        public Shop_T()
        {
            this.Menu_T = new HashSet<Menu_T>();
        }
    
        public int ShopID { get; set; }
        public string ShopName { get; set; }
        public string Logo { get; set; }
        public string HeaderPic { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public Nullable<decimal> MinOrder { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<decimal> DeliveryCharge { get; set; }
        public Nullable<int> CurrentStatus { get; set; }
        public Nullable<bool> Branch { get; set; }
        public Nullable<int> MainShopID { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<bool> HasOffers { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<System.DateTime> DOC { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<bool> AcceptsCash { get; set; }
        public Nullable<bool> AcceptsCard { get; set; }
        public string resttype { get; set; }
    
        public virtual ICollection<Menu_T> Menu_T { get; set; }
    }
}
