using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {

            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ViewBag.menuno = 4;
            ViewBag.submenuno = 1;

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.Restaurants = db.Shop_T.OrderBy(x => x.ShopName).ToList();
                db.Dispose();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ReportView(FormCollection col)
        {

            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ViewBag.menuno = 4;
            ViewBag.submenuno = 1;


            List<Order> orders;
            List<Order> customers;
            List<Order> shops;

            int storeid = Convert.ToInt32(col["ShopID"]);
            DateTime fromx = Convert.ToDateTime(col["from"]);
            DateTime tox = Convert.ToDateTime(col["to"]).AddDays(1);


            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.Restaurants = db.Shop_T.OrderBy(x => x.ShopName).ToList();

                orders = (from o in db.Order_T
                          join od in db.OrderDetail_T on o.OrderID equals od.OrderID
                          join s in db.Status_T on o.CurrentStatusID equals s.StatusID
                          where o.StoreID == storeid && o.OrderDate >= fromx && o.OrderDate <= tox && o.CurrentStatusID == 5
                          orderby o.OrderID descending
                          select new Order { OrderID = o.OrderID, Menu = od.Menu, Details = od.Description, Amount = od.Amount, Quantity = od.Quantity, Status = s.Status, OrderDate = o.OrderDate, DeliveryAdvice = o.DeliveryAdvice, CurrentStatusID = o.CurrentStatusID, DeliveryCharge = o.DeliveryCharge, OrderTotal = o.GrossAmount, CustomerID = o.CustomerID, ShopID = o.StoreID }
                                ).ToList();


                var custids = orders.Select(x => x.CustomerID).ToList();
                var shpids = orders.Select(x => x.ShopID).ToList();


                shops = (from sh in db.Shop_T
                         where shpids.Contains(sh.ShopID)
                         select new Order { ShopID = sh.ShopID, ShopName = sh.ShopName, Discount = sh.Discount }
                               ).ToList();


                using (TMain tm = new TMain())
                {
                    customers = (from c in tm.Customer_T
                                 where custids.Contains(c.CustomerID)
                                 select new Order { Name = c.Name, Phone = c.Mobile, Email = c.Email, FlatVilla = c.FlatVilla, Building = c.Building, Road = c.Road, Block = c.Block, Location = c.Location, CustomerID = c.CustomerID }
                                                 ).ToList();

                    ViewBag.OrderDCS = (from ox in orders
                                        join cx in customers on ox.CustomerID equals cx.CustomerID
                                        join sh in shops on ox.ShopID equals sh.ShopID
                                        select new Order { OrderID = ox.OrderID, Menu = ox.Menu, Details = ox.Details, Amount = ox.Amount, Quantity = ox.Quantity, Status = ox.Status, OrderDate = ox.OrderDate, DeliveryAdvice = ox.DeliveryAdvice, CurrentStatusID = ox.CurrentStatusID, DeliveryCharge = ox.DeliveryCharge, OrderTotal = ox.OrderTotal, CustomerID = ox.CustomerID, Name = cx.Name, Phone = cx.Phone, Email = cx.Email, FlatVilla = cx.FlatVilla, Building = cx.Building, Road = cx.Road, Block = cx.Block, Location = cx.Location, ShopName = sh.ShopName, Discount = sh.Discount }
                                           ).ToList();
                    tm.Dispose();
                }
                db.Dispose();
            }


            return View();
        }

    }
}
