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
    public class LocationController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Location/

        public ActionResult Index()
        {
            ViewBag.menuno = 5;
            ViewBag.submenuno = 1;

            return View(db.Location_T.ToList());
        }

        //
        // GET: /Location/Details/5

        public ActionResult Details(int id = 0)
        {
            Location_T location_t = db.Location_T.Find(id);
            if (location_t == null)
            {
                return HttpNotFound();
            }
            return View(location_t);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Location/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location_T location_t)
        {
            if (ModelState.IsValid)
            {
                db.Location_T.Add(location_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location_t);
        }

        //
        // GET: /Location/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location_T location_t = db.Location_T.Find(id);
            if (location_t == null)
            {
                return HttpNotFound();
            }
            return View(location_t);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location_T location_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location_t);
        }

        //
        // GET: /Location/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Location_T location_t = db.Location_T.Find(id);
            if (location_t == null)
            {
                return HttpNotFound();
            }
            return View(location_t);
        }

        //
        // POST: /Location/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location_T location_t = db.Location_T.Find(id);
            db.Location_T.Remove(location_t);
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