using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class DefaultController : Controller
    {
        private TauseelEatEntities db = new TauseelEatEntities();
        //
        // GET: /Default/

        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login"); }
            ViewBag.menuno = 1;
            return View();
        }

 

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            if (col["userid"] == "Tauseeloffice" && col["password"] == "manama2#16")
            {
                Session["LoggedIn"] = "YES";

                return RedirectToAction("../Shop");
            }
            else {
                ViewBag.Results = "Invalid User ID / Password";
            }

            return View();
        }

       
        public ActionResult Logout()
        {
           
                Session["LoggedIn"] = null;

                return RedirectToAction("Index");
            
        }


        

       
        


    }
}
