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
    public class MenuTypeController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /MenuType/

        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            ViewBag.menuno = 6;
            ViewBag.submenuno = 5;

            int ShopID = Convert.ToInt32(Session["CurrentShopID"]);

            ViewBag.shopname = db.Shop_T.Where(x => x.ShopID == ShopID).Select(x => x.ShopName).SingleOrDefault();
             



            return View(db.MenuType_T.Where(x => x.ShopID == ShopID).ToList());
        }

        
        //
        // POST: /MenuType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuType_T menutype_t)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            if (ModelState.IsValid)
            {
                db.MenuType_T.Add(menutype_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menutype_t);
        }

        //
        // GET: /MenuType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            MenuType_T menutype_t = db.MenuType_T.Find(id);
            if (menutype_t == null)
            {
                return HttpNotFound();
            }
            return View(menutype_t);
        }

        //
        // POST: /MenuType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuType_T menutype_t)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            if (ModelState.IsValid)
            {
                db.Entry(menutype_t).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(menutype_t);
        }

        //
        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}