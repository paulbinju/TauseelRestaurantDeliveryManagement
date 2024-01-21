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
    public class CuisineController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Cuisine/

        public ActionResult Index()
        {
            ViewBag.menuno = 5;
            ViewBag.submenuno = 2;

            return View(db.Cuisine_T.ToList());
        }

        //
        // GET: /Cuisine/Details/5

        public ActionResult Details(int id = 0)
        {
            Cuisine_T cuisine_t = db.Cuisine_T.Find(id);
            if (cuisine_t == null)
            {
                return HttpNotFound();
            }
            return View(cuisine_t);
        }

        //
        // GET: /Cuisine/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cuisine/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cuisine_T cuisine_t)
        {
            if (ModelState.IsValid)
            {
                db.Cuisine_T.Add(cuisine_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cuisine_t);
        }

        //
        // GET: /Cuisine/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cuisine_T cuisine_t = db.Cuisine_T.Find(id);
            if (cuisine_t == null)
            {
                return HttpNotFound();
            }
            return View(cuisine_t);
        }

        //
        // POST: /Cuisine/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cuisine_T cuisine_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuisine_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cuisine_t);
        }

        //
        // GET: /Cuisine/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cuisine_T cuisine_t = db.Cuisine_T.Find(id);
            if (cuisine_t == null)
            {
                return HttpNotFound();
            }
            return View(cuisine_t);
        }

        //
        // POST: /Cuisine/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuisine_T cuisine_t = db.Cuisine_T.Find(id);
            db.Cuisine_T.Remove(cuisine_t);
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