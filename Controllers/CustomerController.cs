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
    public class CustomerController : Controller
    {
        private TMain db = new TMain();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            ViewBag.menuno = 3;
            ViewBag.submenuno = 1;

            return View(db.Customer_T.OrderByDescending(x => x.CustomerID).Take(100).ToList());
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer_T customer_t = db.Customer_T.Find(id);
            if (customer_t == null)
            {
                return HttpNotFound();
            }
            return View(customer_t);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer_T customer_t)
        {
            if (ModelState.IsValid)
            {
                db.Customer_T.Add(customer_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer_t);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer_T customer_t = db.Customer_T.Find(id);
            if (customer_t == null)
            {
                return HttpNotFound();
            }
            return View(customer_t);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer_T customer_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer_t);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer_T customer_t = db.Customer_T.Find(id);
            if (customer_t == null)
            {
                return HttpNotFound();
            }
            return View(customer_t);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer_T customer_t = db.Customer_T.Find(id);
            db.Customer_T.Remove(customer_t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}