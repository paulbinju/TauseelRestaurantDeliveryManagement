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
    public class oldMenuController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();

        //
        // GET: /Menu/

        public ActionResult Index(int? id)
        {
            if (Session["RLoggedIn"] == null) { return RedirectToAction("Login", "Delivery"); }

            ViewBag.menuno = 5;
            ViewBag.submenuno = 1;



            ViewBag.ShopID = new SelectList(db.Shop_T.Where(x => x.ShopID > 0), "ShopID", "ShopName");
            ViewBag.MenuTypeID = new SelectList(db.MenuType_T, "MenuTypeID", "MenuType", "MenuTypeID");

            var menu_t = db.Menu_T.Include(m => m.Shop_T).OrderByDescending(x => x.ShopID).Take(0);

            if (id != null)
            {
                menu_t = db.Menu_T.Include(m => m.Shop_T).Where(x => x.ShopID == id).OrderBy(x => x.SequenceNo);
            }
            


            return View(menu_t.ToList());
        }


        


        [HttpPost]
        public ActionResult Filterx(FormCollection col) {
            return RedirectToAction("../Menus/" + col["ShopID"]);
        }


        //
        // GET: /Menu/Details/5

        public ActionResult Details(int id = 0)
        {
            Menu_T menu_t = db.Menu_T.Find(id);
            if (menu_t == null)
            {
                return HttpNotFound();
            }
            return View(menu_t);
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            ViewBag.ShopID = new SelectList(db.Shop_T, "ShopID", "ShopName");
            return View();
        }

        //
        // POST: /Menu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu_T menu_t)
        {
            if (ModelState.IsValid)
            {
             
                db.Menu_T.Add(menu_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShopID = new SelectList(db.Shop_T, "ShopID", "ShopName", menu_t.ShopID);
            return View(menu_t);
        }

        //
        // GET: /Menu/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    Menu_T menu_t = db.Menu_T.Find(id);
        //    if (menu_t == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShopID = new SelectList(db.Shop_T, "ShopID", "ShopName", menu_t.ShopID);
        //    return View(menu_t);
        //}

        //
        // POST: /Menu/Edit/5

        [HttpPost]
        public ActionResult Edit(Menu_T menu_t)
        {
            
        
                db.Entry(menu_t).State = EntityState.Modified;
           
                db.SaveChanges();

                return RedirectToAction("../Menus/" + menu_t.ShopID);
        }

        //
        // GET: /Menu/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Menu_T menu_t = db.Menu_T.Find(id);
        //    if (menu_t == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(menu_t);
        //}

        //
        // POST: /Menu/Delete/5

        [HttpPost]
        public ActionResult Delete(FormCollection col)
        {
            Menu_T menu_t = db.Menu_T.Find(Convert.ToInt32(col["MenuID"]));
            db.Menu_T.Remove(menu_t);
            db.SaveChanges();
            return RedirectToAction("../Menus/" + menu_t.ShopID);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}