using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class ShopController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Shop/

        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            ViewBag.menuno = 6;
            ViewBag.submenuno = 1;

            ViewBag.Shops = (from s in db.Shop_T
                             where s.ShopID != 0
                             orderby s.ShopID descending
                             select new Shop { ShopID = s.ShopID, ShopName = s.ShopName, HeaderPic = s.HeaderPic, Logo = s.Logo, Address = s.Address }
                              ).ToList();
                
                
                db.Shop_T.Where(x => x.ShopID != 0);

            ViewBag.CategoryID = new SelectList(db.Category_T, "CategoryID", "Category");

            return View();
        }

        //
        // GET: /Shop/Details/5

        public ActionResult Details(int id)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ViewBag.menuno = 6;
            ViewBag.submenuno = 1;

            Session["CurrentShopID"] = id;

            ViewBag.ShopID = id;
            Shop_T shop_t = db.Shop_T.Find(id);

            ViewBag.locations = db.Location_T.OrderBy(x => x.Location).ToList();
            ViewBag.cuisine = db.Cuisine_T.OrderBy(x => x.Cuisine).ToList();

            ViewBag.Shops = (from s in db.Shop_T
                             where s.ShopID == id
                             select new Shop { ShopID = s.ShopID, ShopName = s.ShopName, HeaderPic = s.HeaderPic, Logo = s.Logo, Active = s.Active, MinOrder = s.MinOrder, Password = s.Password, Userid = s.Userid, DeliveryCharge = s.DeliveryCharge, CurrentStatus = s.CurrentStatus, HasOffers = s.HasOffers, Discount = s.Discount, DeliveryTime = s.DeliveryTime, AcceptsCard = s.AcceptsCard, AcceptsCash = s.AcceptsCash, Address= s.Address }
                          ).ToList();

            ViewBag.AllShops = (from s in db.Shop_T
                                orderby s.ShopName
                                select s
                               );


            int MainShopID = (from s in db.ShopMapping_T
                              where s.ShopID == id
                              select s.MainShopID
                                   ).SingleOrDefault();

            ViewBag.MappedShop = MainShopID;


            ViewBag.Showlocations = (from sl in db.ShopLocation_T
                                     join l in db.Location_T on sl.LocationID equals l.LocationID
                                     where sl.ShopID == id
                                     orderby l.Location
                                     select new Locations { LocationName = l.Location, LocationID = l.LocationID, LocDeliveryCharge= sl.LocDeliveryCharge, LocDeliveryTime= sl.LocDeliveryTime, LocMinOrder= sl.LocMinOrder }
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


            return View(shop_t);
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
            return RedirectToAction("Details/" + col["shopid"]);
        }



        //
        // GET: /Shop/Create

        public ActionResult Create()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            ViewBag.CategoryID = new SelectList(db.Category_T, "CatID", "Category");
            return View();
        }

        //
        // POST: /Shop/Create

        [HttpPost]
        public ActionResult Create(Shop_T shop_t)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            shop_t.Active = false;
            shop_t.CurrentStatus = 2;
            shop_t.Branch = false;
            shop_t.HasOffers = false;
            shop_t.Discount = 0;
            shop_t.AcceptsCard = false;
            shop_t.AcceptsCash = true;
            shop_t.DeliveryTime = "45";

            shop_t.DOC = DateTime.UtcNow;
            db.Shop_T.Add(shop_t);
            db.SaveChanges();

            return RedirectToAction("Index");


        }

        //
        // GET: /Shop/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            Shop_T shop_t = db.Shop_T.Find(id);
            if (shop_t == null)
            {
                return HttpNotFound();
            }
         //   ViewBag.CategoryID = new SelectList(db.Category_T, "CategoryID", "Category", shop_t.CategoryID);
            return View(shop_t);
        }

        //
        // POST: /Shop/Edit/5

        [HttpPost]
        public ActionResult EditLogo(FormCollection col, HttpPostedFileBase pic)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            db.Database.ExecuteSqlCommand("update Shop_t set logo='" + pic.FileName + "' where shopid=" + col["shopid"]);

                if (pic != null && pic.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(pic.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/shoplogos"), col["shopid"] + "-"+ fileName);
                    pic.SaveAs(path);
                }
            return RedirectToAction("Details/" + col["shopid"]);
          
        }

        [HttpPost]
        public ActionResult EditHeader(FormCollection col, HttpPostedFileBase pic)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            db.Database.ExecuteSqlCommand("update Shop_t set HeaderPic='" + pic.FileName + "' where shopid=" + col["shopid"]);

            if (pic != null && pic.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(pic.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/shopheader"), col["shopid"] + "-" + fileName);
                pic.SaveAs(path);
            }
            return RedirectToAction("Details/" + col["shopid"]);

        }

        [HttpPost]
        public ActionResult MainItems(FormCollection col)
        {

            string sql = "update Shop_t set shopname='" + col["ShopName"].Replace("\'", "''") + "',userid='" + col["Userid"] + "',Password='" + col["Password"] + "', minorder='" + col["MinOrder"] + "', active='" + col["active"] + "' , currentstatus='" + col["currentstatus"] + "', deliverycharge=" + col["deliverycharge"] + ", hasoffers=" + col["hasoffers"] + ", DeliveryTime=" + col["DeliveryTime"] + ", AcceptsCash=" + col["AcceptsCash"] + ", AcceptsCard=" + col["AcceptsCard"] + ", discount=" + col["discount"] + ", Address='" + col["Address"] + "' where shopid=" + Convert.ToInt32(col["ShopID"]);
          //  Console.Write(sql);
            db.Database.ExecuteSqlCommand(sql);
            return RedirectToAction("Details/" + col["shopid"]);

        }

        [HttpPost]
        public ActionResult AddLocations(FormCollection col) {


            if (!string.IsNullOrEmpty(col["location"]))
            {

                db.Database.ExecuteSqlCommand("Delete from shoplocation_t where shopid=" + col["ShopID"]);

                string[] temp = col["location"].Split(',');

                string check = col["location"];

                for (int i = 0; i < temp.Length; i++)
                {
                    string locdeliverycharge = col["LocDeliveryCharge-" + temp[i]];
                    string LocDeliveryTime = col["LocDeliveryTime-" + temp[i]];
                    string LocMinOrder = col["LocMinOrder-" + temp[i]];
                    string qury = "Insert into shoplocation_T (Shopid,locationid,locdeliverycharge,locdeliverytime,locminorder) values(" + col["ShopID"] + "," + temp[i] + "," + locdeliverycharge + "," + LocDeliveryTime + "," + LocMinOrder + ")";
                    
                    db.Database.ExecuteSqlCommand(qury);
                }
                 
            }

            return RedirectToAction("Details/" + col["shopid"]);
        }

        [HttpPost]
        public ActionResult AddCuisines(FormCollection col)
        {


            if (!string.IsNullOrEmpty(col["cuisine"]))
            {

                db.Database.ExecuteSqlCommand("Delete from shopcuisine_t where shopid=" + col["ShopID"]);

                string[] temp = col["cuisine"].Split(',');
                for (int i = 0; i < temp.Length; i++)
                {
                    db.Database.ExecuteSqlCommand("Insert into shopcuisine_T (Shopid,cuisineid) values(" + col["ShopID"] + "," + temp[i] + ")");
                }

            }

            return RedirectToAction("Details/" + col["shopid"]);
        }

        [HttpPost]
        public ActionResult MapShops(FormCollection col)
        {
            db.Database.ExecuteSqlCommand("Delete from [ShopMapping_T] where shopid=" + col["ShopID"]);
            db.Database.ExecuteSqlCommand("Insert into [ShopMapping_T] (Shopid,mainshopid) values(" + col["ShopID"] + "," + col["mainshopid"] + ")");

            return RedirectToAction("Details/" + col["shopid"]);
        }

        //
        // GET: /Shop/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            Shop_T shop_t = db.Shop_T.Find(id);
            if (shop_t == null)
            {
                return HttpNotFound();
            }
            return View(shop_t);
        }

        //
        // POST: /Shop/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            db.Database.ExecuteSqlCommand("delShop @shopid={0}", id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}