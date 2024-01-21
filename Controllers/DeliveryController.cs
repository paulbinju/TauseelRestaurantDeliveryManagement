using TauseelRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TauseelRestaurant.Controllers
{
    public class DeliveryController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            SqlConnection.ClearAllPools();

            using (TauseelEatEntities db = new TauseelEatEntities())
            {

                string emailid = Convert.ToString(col["emailid"]).Trim();
                string password = Convert.ToString(col["password"]).Trim();
                var usert = db.Shop_T.Where(x => x.Userid == emailid && x.Password == password && x.Active == true).ToList();

                if (usert.Count != 0)
                {
                    Session["RLoggedIn"] = "YES";
                    Session["Name"] = usert[0].ShopName;
                    Session["ShopID"] = usert[0].ShopID;
                    return RedirectToAction("../Delivery/Orders");
                }
                else
                {
                    ViewBag.Results = "Invalid User ID / Password";
                }


                db.Dispose();
                SqlConnection.ClearAllPools();
            }

            return View();
        }


        public ActionResult Logout()
        {
            Session["RLoggedIn"] = null;
            Session["Name"] = null;
            Session["ShopID"] = null;

            return RedirectToAction("Login", "Delivery");
        }


        public ActionResult Home()
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }

            ViewBag.menuno = 1;
            ViewBag.submenuno = 1;


            int id = Convert.ToInt32(Session["ShopID"]);

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.ShopID = id;
                Shop_T shop_t = db.Shop_T.Find(id);

                ViewBag.locations = db.Location_T.OrderBy(x => x.Location).ToList();
                ViewBag.cuisine = db.Cuisine_T.OrderBy(x => x.Cuisine).ToList();

                ViewBag.Shops = (from s in db.Shop_T
                                 where s.ShopID == id
                                 select new Shop { ShopID = s.ShopID, ShopName = s.ShopName, HeaderPic = s.HeaderPic, Logo = s.Logo, Active = s.Active, MinOrder = s.MinOrder, Password = s.Password, Userid = s.Userid, DeliveryCharge = s.DeliveryCharge, CurrentStatus = s.CurrentStatus, HasOffers = s.HasOffers }
                              ).ToList();


                ViewBag.Showlocations = (from sl in db.ShopLocation_T
                                         join l in db.Location_T on sl.LocationID equals l.LocationID
                                         where sl.ShopID == id
                                         orderby l.Location
                                         select new Locations { LocationName = l.Location, LocationID = l.LocationID }
                                         ).ToList();

                ViewBag.Showcuisines = (from sc in db.ShopCuisine_T
                                        join c in db.Cuisine_T on sc.CuisineID equals c.CuisineID
                                        where sc.ShopID == id
                                        orderby c.Cuisine
                                        select new Cuisines { CuisineName = c.Cuisine, CuisineID = c.CuisineID }
                                      ).ToList();

                ViewBag.Timings = (from t in db.ShopTiming_T
                                   where t.ShopID == id
                                   select t).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();
            }
            return View();
        }

        public ActionResult Orders()
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            ViewBag.menuno = 2;
            ViewBag.submenuno = 1;
            int id = Convert.ToInt32(Session["ShopID"]);
            List<Order> orders;
            List<Order> customers;

            using (TauseelEatEntities db = new TauseelEatEntities())
            {

                orders = (from o in db.Order_T
                          join od in db.OrderDetail_T on o.OrderID equals od.OrderID
                          join s in db.Status_T on o.CurrentStatusID equals s.StatusID
                          where o.StoreID == id & o.CurrentStatusID == 1
                          orderby o.OrderID descending
                          select new Order { OrderID = o.OrderID, Menu = od.Menu, Details = od.Description, Amount = od.Amount, Quantity = od.Quantity, Status = s.Status, OrderDate = o.OrderDate, DeliveryAdvice = o.DeliveryAdvice, CurrentStatusID = o.CurrentStatusID, DeliveryCharge = o.DeliveryCharge, OrderTotal = o.GrossAmount, CustomerID = o.CustomerID, Discount = o.Discount }
                                ).ToList();

                int shopid = Convert.ToInt32(Session["ShopID"]);

                var custids = orders.Select(x => x.CustomerID).ToList();





                using (TMain tm = new TMain())
                {

                    customers = (from c in tm.Customer_T
                                 where custids.Contains(c.CustomerID)
                                 select new Order { Name = c.Name, Phone = c.Mobile, Email = c.Email, FlatVilla = c.FlatVilla, Building = c.Building, Road = c.Road, Block = c.Block, Location = c.Location, CustomerID = c.CustomerID }
                                                 ).ToList();



                    ViewBag.OrderDCS = (from ox in orders
                                        join cx in customers on ox.CustomerID equals cx.CustomerID
                                        select new Order { OrderID = ox.OrderID, Menu = ox.Menu, Details = ox.Details, Amount = ox.Amount, Quantity = ox.Quantity, Status = ox.Status, OrderDate = ox.OrderDate, DeliveryAdvice = ox.DeliveryAdvice, CurrentStatusID = ox.CurrentStatusID, DeliveryCharge = ox.DeliveryCharge, Discount = ox.Discount, OrderTotal = ox.Amount, CustomerID = ox.CustomerID, Name = cx.Name, Phone = cx.Phone, Email = cx.Email, FlatVilla = cx.FlatVilla, Building = cx.Building, Road = cx.Road, Block = cx.Block, Location = cx.Location }
                                           ).ToList();


                    tm.Dispose();


                }


                db.Dispose();

            }







            return View();
        }

        public ActionResult OrdersProcessed()
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            ViewBag.menuno = 2;
            ViewBag.submenuno = 2;
            int id = Convert.ToInt32(Session["ShopID"]);
            List<Order> orders;
            List<Order> customers;
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                orders = (from o in db.Order_T
                          join od in db.OrderDetail_T on o.OrderID equals od.OrderID
                          join s in db.Status_T on o.CurrentStatusID equals s.StatusID
                          where o.StoreID == id & o.CurrentStatusID == 2
                          orderby o.OrderID descending
                          select new Order { OrderID = o.OrderID, Menu = od.Menu, Details = od.Description, Amount = od.Amount, Quantity = od.Quantity, Status = s.Status, OrderDate = o.OrderDate, DeliveryAdvice = o.DeliveryAdvice, CurrentStatusID = o.CurrentStatusID, DeliveryCharge = o.DeliveryCharge, OrderTotal = o.GrossAmount, CustomerID = o.CustomerID, Discount = o.Discount }
                                ).ToList();
                int shopid = Convert.ToInt32(Session["ShopID"]);
                var custids = orders.Select(x => x.CustomerID).ToList();
                using (TMain tm = new TMain())
                {
                    customers = (from c in tm.Customer_T
                                 where custids.Contains(c.CustomerID)
                                 select new Order { Name = c.Name, Phone = c.Mobile, Email = c.Email, FlatVilla = c.FlatVilla, Building = c.Building, Road = c.Road, Block = c.Block, Location = c.Location, CustomerID = c.CustomerID }
                                                 ).ToList();
                    ViewBag.OrderDCS = (from ox in orders
                                        join cx in customers on ox.CustomerID equals cx.CustomerID
                                        select new Order { OrderID = ox.OrderID, Menu = ox.Menu, Details = ox.Details, Amount = ox.Amount, Quantity = ox.Quantity, Status = ox.Status, OrderDate = ox.OrderDate, DeliveryAdvice = ox.DeliveryAdvice, CurrentStatusID = ox.CurrentStatusID, DeliveryCharge = ox.DeliveryCharge, Discount = ox.Discount, OrderTotal = ox.Amount, CustomerID = ox.CustomerID, Name = cx.Name, Phone = cx.Phone, Email = cx.Email, FlatVilla = cx.FlatVilla, Building = cx.Building, Road = cx.Road, Block = cx.Block, Location = cx.Location }
                                           ).ToList();
                    tm.Dispose();
                }
                db.Dispose();
            }
            return View();
        }

        public ActionResult OrderPrint(int orderid)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            ViewBag.menuno = 2;
            ViewBag.submenuno = 2;
            int id = Convert.ToInt32(Session["ShopID"]);
            List<Order> orders;
            List<Order> customers;
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                orders = (from o in db.Order_T
                          join od in db.OrderDetail_T on o.OrderID equals od.OrderID
                          join s in db.Status_T on o.CurrentStatusID equals s.StatusID
                          where o.OrderID == orderid
                          orderby o.OrderID descending
                          select new Order { OrderID = o.OrderID, Menu = od.Menu, Details = od.Description, Amount = od.Amount, Quantity = od.Quantity, Status = s.Status, OrderDate = o.OrderDate, DeliveryAdvice = o.DeliveryAdvice, CurrentStatusID = o.CurrentStatusID, DeliveryCharge = o.DeliveryCharge, OrderTotal = o.GrossAmount, CustomerID = o.CustomerID }
                                ).ToList();
                int shopid = Convert.ToInt32(Session["ShopID"]);
                var custids = orders.Select(x => x.CustomerID).ToList();
                using (TMain tm = new TMain())
                {
                    customers = (from c in tm.Customer_T
                                 where custids.Contains(c.CustomerID)
                                 select new Order { Name = c.Name, Phone = c.Mobile, Email = c.Email, FlatVilla = c.FlatVilla, Building = c.Building, Road = c.Road, Block = c.Block, Location = c.Location, CustomerID = c.CustomerID }
                                                 ).ToList();
                    ViewBag.OrderDCS = (from ox in orders
                                        join cx in customers on ox.CustomerID equals cx.CustomerID
                                        select new Order { OrderID = ox.OrderID, Menu = ox.Menu, Details = ox.Details, Amount = ox.Amount, Quantity = ox.Quantity, Status = ox.Status, OrderDate = ox.OrderDate, DeliveryAdvice = ox.DeliveryAdvice, CurrentStatusID = ox.CurrentStatusID, DeliveryCharge = ox.DeliveryCharge, OrderTotal = ox.Amount, CustomerID = ox.CustomerID, Name = cx.Name, Phone = cx.Phone, Email = cx.Email, FlatVilla = cx.FlatVilla, Building = cx.Building, Road = cx.Road, Block = cx.Block, Location = cx.Location }
                                           ).ToList();
                    tm.Dispose();
                }
                db.Dispose();
            }
            return View();
        }


        public ActionResult Report() {

            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            return View();
        }


        [HttpPost]
        public ActionResult ReportView(FormCollection col)
        {

            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            ViewBag.menuno = 4;
            ViewBag.submenuno = 1;
            int id = Convert.ToInt32(Session["ShopID"]);


            List<Order> orders;
            List<Order> customers;
            List<Order> shops;

       
            DateTime fromx = Convert.ToDateTime(col["from"]);
            DateTime tox = Convert.ToDateTime(col["to"]).AddDays(1);


            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.Restaurants = db.Shop_T.OrderBy(x => x.ShopName).ToList();

                orders = (from o in db.Order_T
                          join od in db.OrderDetail_T on o.OrderID equals od.OrderID
                          join s in db.Status_T on o.CurrentStatusID equals s.StatusID
                          where o.StoreID == id && o.OrderDate >= fromx && o.OrderDate <= tox && o.CurrentStatusID == 5
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


        public ActionResult OrderAccept(FormCollection col)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("update order_T set currentstatusid=2 where orderid=" + col["orderid"]);
                db.Dispose();
            }
            return RedirectToAction("Orders");
        }

        public ActionResult OrderDelivered(FormCollection col)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("update order_T set currentstatusid=5 where orderid=" + col["orderid"]);
                db.Dispose();
            }
            return RedirectToAction("OrdersProcessed");
        }


        [HttpPost]
        public ActionResult EditLogo(FormCollection col, HttpPostedFileBase pic)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("update Shop_t set headerpic='" + pic.FileName + "' where shopid=" + col["shopid"]);
            }
            if (pic != null && pic.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(pic.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/shoplogos"), col["shopid"] + "-" + fileName);
                pic.SaveAs(path);
            }
            return RedirectToAction("Home", "Delivery");

        }

        [HttpPost]
        public ActionResult EditPic(FormCollection col, HttpPostedFileBase pic)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            if (pic != null && pic.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(pic.FileName);

                var path = Path.Combine(Server.MapPath("~/shoplogos"), "menupic-" + col["shopid"] + "-" + col["menuid"] + ".jpg");
                pic.SaveAs(path);
            }
            return RedirectToAction("Home", "Delivery");

        }


        [HttpPost]
        public ActionResult MainItems(FormCollection col)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                //  string sqlqry = "update Shop_t set shopname='" + col["ShopName"] + "',userid='" + col["Userid"] + "',Password='" + col["Password"] + "', minorder='" + col["MinOrder"] + "' , deliverycharge=" + col["deliverycharge"] + " , CurrentStatus='" + col["CurrentStatus"] + "', HasOffers='" + col["HasOffers"] + "'  where shopid=" + col["ShopID"];
                string sqlqry = "update Shop_t set CurrentStatus='" + col["CurrentStatus"] + "'  where shopid=" + col["ShopID"];
                Console.WriteLine(sqlqry);
                db.Database.ExecuteSqlCommand(sqlqry);

            }
            return RedirectToAction("Home", "Delivery");

        }
        [HttpPost]
        public ActionResult AddLocations(FormCollection col)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }

            if (!string.IsNullOrEmpty(col["location"]))
            {
                using (TauseelEatEntities db = new TauseelEatEntities())
                {
                    db.Database.ExecuteSqlCommand("Delete from shoplocation_t where shopid=" + col["ShopID"]);

                    string[] temp = col["location"].Split(',');
                    for (int i = 0; i < temp.Length; i++)
                    {
                        db.Database.ExecuteSqlCommand("Insert into shoplocation_T (Shopid,locationid) values(" + col["ShopID"] + "," + temp[i] + ")");
                    }
                }
            }

            return RedirectToAction("Home", "Delivery");

        }


        [HttpPost]
        public ActionResult AddCuisines(FormCollection col)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }

            if (!string.IsNullOrEmpty(col["cuisine"]))
            {
                using (TauseelEatEntities db = new TauseelEatEntities())
                {
                    db.Database.ExecuteSqlCommand("Delete from shopcuisine_t where shopid=" + col["ShopID"]);

                    string[] temp = col["cuisine"].Split(',');
                    for (int i = 0; i < temp.Length; i++)
                    {
                        db.Database.ExecuteSqlCommand("Insert into shopcuisine_T (Shopid,cuisineid) values(" + col["ShopID"] + "," + temp[i] + ")");
                    }

                    db.Dispose();
                    SqlConnection.ClearAllPools();
                }
            }

            return RedirectToAction("Home", "Delivery");

        }

        [HttpPost]
        public ActionResult AddTimings(FormCollection col)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("Delete from [ShopTiming_T]  where shopid=" + col["ShopID"]);
                for (int x = 1; x <= 7; x++)
                {
                    db.Database.ExecuteSqlCommand("insert into [ShopTiming_T] (shopid,day,starttime,endtime) values(" + col["ShopID"] + ",'" + col["day" + x] + "','" + col["starttime" + x] + "','" + col["endtime" + x] + "')");
                }
            }
            return RedirectToAction("Home", "Delivery");
        }
    }
}
