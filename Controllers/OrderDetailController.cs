using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class OrderDetailController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /OrderDetail/

        public ActionResult Index(int orderid)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            var orderdetail_t = db.OrderDetail_T.Include(o => o.Order_T).ToList();

 

            ViewBag.StatusID = new SelectList(db.Status_T.Where(x=>x.StatusID>4), "StatusID", "Status");
            ViewBag.OrderDetail = db.OrderDetail_T.Where(x => x.OrderID == orderid).ToList();
            ViewBag.CustomerDetails = (from o in db.Order_T
                                       join c in db.ResCustomer_T on o.CustomerID equals c.CustomerID
                                       where o.OrderID == orderid
                                       select new Order { OrderID = o.OrderID, OrderDate = o.OrderDate,  CustomerID = c.CustomerID, FlatVilla = c.FlatVilla, Building = c.Building, Road = c.Road, Block = c.Block, Email = c.Email, Location = c.Location, Mobile = c.Mobile, Phone = c.Phone, Name = c.Name });

            ViewBag.OrderID = orderid;

            return View( );
        }


     


        //
        // GET: /OrderDetail/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            OrderDetail_T orderdetail_t = db.OrderDetail_T.Find(id);
            if (orderdetail_t == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail_t);
        }

        //
        // GET: /OrderDetail/Create

        public ActionResult Create()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ViewBag.OrderID = new SelectList(db.Order_T, "OrderID", "OrderID");
            return View();
        }

        //
        // POST: /OrderDetail/Create

   

        //
        // GET: /OrderDetail/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            OrderDetail_T orderdetail_t = db.OrderDetail_T.Find(id);
            if (orderdetail_t == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Order_T, "OrderID", "OrderID", orderdetail_t.OrderID);
            return View(orderdetail_t);
        }

        //
        // POST: /OrderDetail/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetail_T orderdetail_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetail_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Order_T, "OrderID", "OrderID", orderdetail_t.OrderID);
            return View(orderdetail_t);
        }

        //
        // GET: /OrderDetail/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            OrderDetail_T orderdetail_t = db.OrderDetail_T.Find(id);
            if (orderdetail_t == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail_t);
        }

        //
        // POST: /OrderDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail_T orderdetail_t = db.OrderDetail_T.Find(id);
            db.OrderDetail_T.Remove(orderdetail_t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ReportSummary()
        {

            ViewBag.Report = db.Database.SqlQuery<ReportSummary>(@"select shopname,storecharge, servicecharge from  [dbo].[Order_T] o
                join orderdetail_t od on o.orderid=od.orderid
                where 1=2");


            ViewBag.ReportTotal = db.Database.SqlQuery<ReportSummary>(@"select storecharge, servicecharge from  [dbo].[Order_T] o
                    join orderdetail_t od on o.orderid=od.orderid
                    where 1=2");




            return View();
        }

       [HttpPost]
        public ActionResult ReportSummary(FormCollection col)
        {
            string fromx = Convert.ToString(col["from"]).Trim();
            DateTime newDate = DateTime.ParseExact(fromx, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string fromdate = newDate.ToString("yyyy-MM-dd");

            string tox = Convert.ToString(col["to"]).Trim();
            DateTime newtoDate = DateTime.ParseExact(tox, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string todate = newtoDate.ToString("yyyy-MM-dd");


            @ViewBag.Dates = fromdate + " - " + todate;
           
           ViewBag.Report = db.Database.SqlQuery<ReportSummary>("select shopname,sum(storecharge) as storecharge,sum(servicecharge) as servicecharge from  [dbo].[Order_T] o join orderdetail_t od on o.orderid=od.orderid    where currentstatusid=5  and (convert(date,orderdate) >= convert(date,'" + fromdate + "') and convert(date,orderdate)<= convert(date,'" + todate + "'))  group by shopname");
           ViewBag.ReportTotal = db.Database.SqlQuery<ReportSummary>("select sum(storecharge) as storecharge,sum(servicecharge) as servicecharge from  [dbo].[Order_T] o  join orderdetail_t od on o.orderid=od.orderid where currentstatusid=5  and (convert(date,orderdate) >= convert(date,'" + fromdate + "') and convert(date,orderdate)<= convert(date,'" + todate + "'))");



 



            return View();
        }
        public ActionResult ReportDetails() {

            ViewBag.ShopID = new SelectList(db.Shop_T, "ShopID", "ShopName");

            ViewBag.Report = db.Database.SqlQuery<ReportSummary>(@"select shopname,storecharge, servicecharge from  [dbo].[Order_T] o
                join orderdetail_t od on o.orderid=od.orderid
                where 1=2");


            ViewBag.ReportTotal = db.Database.SqlQuery<ReportSummary>(@"select storecharge, servicecharge from  [dbo].[Order_T] o
                    join orderdetail_t od on o.orderid=od.orderid
                    where 1=2");
            return View();
        }

        [HttpPost]
        public ActionResult ReportDetails(FormCollection col)
        {

            ViewBag.ShopID = new SelectList(db.Shop_T, "ShopID", "ShopName");


            string fromx = Convert.ToString(col["from"]).Trim();
            DateTime newDate = DateTime.ParseExact(fromx, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string fromdate = newDate.ToString("yyyy-MM-dd");

            string tox = Convert.ToString(col["to"]).Trim();
            DateTime newtoDate = DateTime.ParseExact(tox, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string todate = newtoDate.ToString("yyyy-MM-dd");

            string shopid = Convert.ToString(col["ShopID"]);


            @ViewBag.Dates = fromdate + " - " + todate;

            
            ViewBag.Report = db.Database.SqlQuery<ReportSummary>("select name,sum(storecharge) as storecharge,sum(servicecharge) as servicecharge from  [dbo].[Order_T] o join orderdetail_t od on o.orderid=od.orderid  join ResCustomer_T c on c.customerid = o.customerid   where currentstatusid=5  and (convert(date,orderdate) >= convert(date,'" + fromdate + "') and convert(date,orderdate)<= convert(date,'" + todate + "')) and shopid = " + shopid + "  group by name");
            ViewBag.ReportTotal = db.Database.SqlQuery<ReportSummary>("select sum(storecharge) as storecharge,sum(servicecharge) as servicecharge from  [dbo].[Order_T] o  join orderdetail_t od on o.orderid=od.orderid join ResCustomer_T c on c.customerid = o.customerid where currentstatusid=5  and (convert(date,orderdate) >= convert(date,'" + fromdate + "') and convert(date,orderdate)<= convert(date,'" + todate + "')) and shopid=" + shopid + "");




            return View();
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}