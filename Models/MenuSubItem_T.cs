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
    
    public partial class MenuSubItem_T
    {
        public int MenuSubItemID { get; set; }
        public Nullable<int> MenuSubItemGroupID { get; set; }
        public string MenuSubItem { get; set; }
        public Nullable<bool> MultipleSelectable { get; set; }
        public Nullable<bool> Paid { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}