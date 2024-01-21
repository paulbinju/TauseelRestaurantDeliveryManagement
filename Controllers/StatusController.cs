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
    public class StatusController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Status/

        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            return View(db.Status_T.ToList());
        }

        //
        // GET: /Status/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            Status_T status_t = db.Status_T.Find(id);
            if (status_t == null)
            {
                return HttpNotFound();
            }
            return View(status_t);
        }

        //
        // GET: /Status/Create

        public ActionResult Create()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            return View();
        }

        //
        // POST: /Status/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Status_T status_t)
        {
            if (ModelState.IsValid)
            {
                db.Status_T.Add(status_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(status_t);
        }

        //
        // GET: /Status/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            Status_T status_t = db.Status_T.Find(id);
            if (status_t == null)
            {
                return HttpNotFound();
            }
            return View(status_t);
        }

        //
        // POST: /Status/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Status_T status_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(status_t);
        }

        //
        // GET: /Status/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            Status_T status_t = db.Status_T.Find(id);
            if (status_t == null)
            {
                return HttpNotFound();
            }
            return View(status_t);
        }

        //
        // POST: /Status/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status_T status_t = db.Status_T.Find(id);
            db.Status_T.Remove(status_t);
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