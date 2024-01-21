using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class oldCustomerController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            ViewBag.menuno = 3;
            ViewBag.submenuno = 1;

            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            return View(db.ResCustomer_T.OrderByDescending(x=>x.CustomerID).ToList());
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ResCustomer_T ResCustomer_T = db.ResCustomer_T.Find(id);
            if (ResCustomer_T == null)
            {
                return HttpNotFound();
            }
            return View(ResCustomer_T);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResCustomer_T ResCustomer_T)
        {
            ResCustomer_T.DOC = DateTime.Now;
            db.ResCustomer_T.Add(ResCustomer_T);
            db.SaveChanges();
            return RedirectToAction("Index");
        }






        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ResCustomer_T ResCustomer_T = db.ResCustomer_T.Find(id);
            if (ResCustomer_T == null)
            {
                return HttpNotFound();
            }
            return View(ResCustomer_T);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ResCustomer_T ResCustomer_T)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ResCustomer_T).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ResCustomer_T);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ResCustomer_T ResCustomer_T = db.ResCustomer_T.Find(id);
            if (ResCustomer_T == null)
            {
                return HttpNotFound();
            }
            return View(ResCustomer_T);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            db.Database.ExecuteSqlCommand("delCustomer @customerid={0}", id);

            //ResCustomer_T ResCustomer_T = db.ResCustomer_T.Find(id);
            //db.ResCustomer_T.Remove(ResCustomer_T);
            //db.SaveChanges();

            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(FormCollection col)
        {

            List<ResCustomer_T> ResCustomer_T = new List<ResCustomer_T>();

            string keyword = Convert.ToString(col["keyword"]);

            if (col["colname"] == "Name")
            {
                ResCustomer_T = db.ResCustomer_T.Where(x => x.Name.Contains(keyword)).ToList();
            }
            else if (col["colname"] == "Email")
            {
                ResCustomer_T = db.ResCustomer_T.Where(x => x.Email == keyword).ToList();
            }
            else if (col["colname"] == "Mobile")
            {
                ResCustomer_T = db.ResCustomer_T.Where(x => x.Mobile == keyword).ToList();
            }

            ViewBag.Customers = ResCustomer_T;

            return View();
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    
}