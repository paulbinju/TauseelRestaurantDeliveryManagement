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
    
    public partial class MenuType_T
    {
        public MenuType_T()
        {
            this.Menu_T = new HashSet<Menu_T>();
        }
    
        public int MenuTypeID { get; set; }
        public Nullable<int> ShopID { get; set; }
        public string MenuType { get; set; }
    
        public virtual ICollection<Menu_T> Menu_T { get; set; }
    }
}