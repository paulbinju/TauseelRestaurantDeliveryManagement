using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauseelRestaurant.Models
{
    public class OrderStatus
    {
        public int? OrderID { get; set; }
        public string Status { get; set; }
        public DateTime? TimeStamp { get; set; }
    }

    public class Customers {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string FlatVilla { get; set; }
        public string Building { get; set; }
        public string Road { get; set; }
        public string Block { get; set; }
        public string Location { get; set; }
        public string Landmark { get; set; }
    }

    public class TauseelOpen {
        public string day { get; set; }
        public string hour { get; set; }
        public string Status { get; set; }
    }
    public class Cateogory {
        public int CategoryID { get; set; }
        public string Category { get; set; }
    
    }
    public class customercount {
        public int count { get; set; }
    }
    public class Bills {
        public int BillID { get; set; }
        public int? OrderID { get; set; }
    
    }

    public class Shop {
        public int ShopID { get; set; }
        public string ShopName { get; set; }
        public string HeaderPic { get; set; }
        public string Logo { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public decimal? MinOrder { get; set; }
        public bool? Active { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public int? CuisineID { get; set; }
        public string Cuisine { get; set; }
        public int? CurrentStatus { get; set; }
        public string CurrentStatusText { get; set; }
        public string Address { get; set; }
        public bool? HasOffers { get; set; }
        public string PhoneNo { get; set; }
        public decimal? Discount { get; set; }
        public string DeliveryTime { get; set; }
        public bool? AcceptsCash { get; set; }
        public bool? AcceptsCard { get; set; }

    }


    public class ShopCuisines
    {
        public int? ShopID { get; set; }
        public int CuisineID { get; set; }
        public string Cuisine { get; set; }
    }

    public class ShopLocations
    {
        public int? ShopID { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public decimal? MinOrder { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public string DeliveryTime { get; set; }
    }
    public class ShopTimings
    {
        public int? ShopID { get; set; }
        public string Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
    public class Menus
    {
        public int MenuID { get; set; }
        public int? ShopID { get; set; }
        public int? MenuTypeID { get; set; }
        public string MenuType { get; set; }
        public string Menu { get; set; }
        public string Description { get; set; }
        public double? Rate { get; set; }
        public int Qty { get; set; }
        public int MenuSubMenuItemGroupID { get; set; }
        public string MenuSubItemGroup { get; set; }
        public string MenuSubItem { get; set; }
    }

    public class MenuSubMenu
    {
        public int MenuID { get; set; }
        public int MenuSubItemGroupID { get; set; }
        public string MenuSubItemGroup { get; set; }
        public string MenuSubItem { get; set; }
    }

    public class SubMenu {

        public int MenuSubItemGroupID { get; set; }
        public int? ShopID { get; set; }
        public string MenuSubItemGroup { get; set; }
        public int MenuSubItemID { get; set; }
        public string MenuSubItem { get; set; }
        public bool? MultipleSelectable { get; set; }
        public bool? Paid { get; set; }
        public decimal? Amount { get; set; }
        public int MenuID { get; set; }




    }

    public class ReportSummary {

        public string Name { get; set; }
        public string ShopName { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal StoreCharge { get; set; }

    }


    public class Order {
        public int? OrderID { get; set; }
        public int OrderDetailID { get; set; }
        public int? ShopID { get; set; }
        public string ShopName { get; set; }
        public string Menu { get; set; }
        public string Details { get; set; }
        public decimal? Amount { get; set; }
        public int? Quantity { get; set; }
        public int? StatusID { get; set; }
        public string  Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? StoreCharge { get; set; }
        public decimal? ServiceCharge { get; set; }
        public string DeliveryAdvice { get; set; }
        public int? CurrentStatusID { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public decimal? OrderTotal { get; set; }
        public decimal? Discount { get; set; }

        public int? CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string FlatVilla { get; set; }
        public string Building { get; set; }
        public string Road { get; set; }
        public string Block { get; set; }
        public string Location { get; set; }
        public string Landmark { get; set; }

        public string ShopPhone { get; set; }

    }

    public class Locations {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public decimal? LocDeliveryCharge { get; set; }
        public string LocDeliveryTime { get; set; }
        public decimal? LocMinOrder { get; set; }
    }
    public class Cuisines
    {
        public int CuisineID { get; set; }
        public string CuisineName { get; set; }
    }
}