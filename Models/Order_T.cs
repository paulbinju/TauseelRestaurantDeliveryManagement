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
    
    public partial class Order_T
    {
        public Order_T()
        {
            this.OrderDetail_T = new HashSet<OrderDetail_T>();
        }
    
        public int OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string DeliveryAdvice { get; set; }
        public Nullable<int> CurrentStatusID { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> DeliveryCharge { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string CustAddress { get; set; }
    
        public virtual Status_T Status_T { get; set; }
        public virtual Status_T Status_T1 { get; set; }
        public virtual ICollection<OrderDetail_T> OrderDetail_T { get; set; }
    }
}