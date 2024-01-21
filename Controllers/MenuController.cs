using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class MenuController : Controller
    {

        [HttpPost]
        public ActionResult SubMenuAdd(FormCollection col)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Menu"); }

            if (!string.IsNullOrEmpty(col["groupid"]))
            {
                using (TauseelEatEntities db = new TauseelEatEntities())
                {
                    db.Database.ExecuteSqlCommand("Delete from [Menu-MenuSubItemGroup_T] where MenuID=" + col["MenuID"]);

                    string[] temp = col["groupid"].Split(',');
                    for (int i = 0; i < temp.Length; i++)
                    {
                        db.Database.ExecuteSqlCommand("Insert into [Menu-MenuSubItemGroup_T] (MenuID,MenuSubItemGroupID) values(" + col["MenuID"] + "," + temp[i] + ")");
                    }
                }
            }

            return RedirectToAction("Menu", "Menu");

        }


        [HttpPost]
        public ActionResult AddCuisines(FormCollection col)
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }

            if (!string.IsNullOrEmpty(col["cuisine"]))
            {
                using (TauseelEatEntities db = new TauseelEatEntities())
                {
                    db.Database.ExecuteSqlCommand("Delete from shopcuisine_t where shopid=" + col["ShopID"]);

                    string[] temp = col["cuisine"].Split(',');
                    for (int i = 0; i < temp.Length; i++)
                    {
                        db.Database.ExecuteSqlCommand("Insert into shopcuisine_T (Shopid,cuisineid) values(" + col["ShopID"] + "," + temp[i] + ")");
                    }

                    db.Dispose();
                    SqlConnection.ClearAllPools();
                }
            }

            return RedirectToAction("Home");

        }


        public ActionResult Menu()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Default"); }
            ViewBag.menuno = 6;
            ViewBag.submenuno = 1;

            int ShopID = Convert.ToInt32(Session["CurrentShopID"]);

            


            ViewBag.ShopID = ShopID;
            using (TauseelEatEntities db = new TauseelEatEntities())
            {


                ViewBag.shopname = db.Shop_T.Where(x => x.ShopID == ShopID).Select(x => x.ShopName).SingleOrDefault();



                List<Menus> menus = (from m in db.Menu_T
                                     join mt in db.MenuType_T on m.MenuTypeID equals mt.MenuTypeID
                                     where m.ShopID == ShopID
                                     select new Menus { MenuType = mt.MenuType, Menu = m.Menu, Description = m.Description, Rate = m.Rate, MenuID = m.MenuID, ShopID = m.ShopID, MenuTypeID = m.MenuTypeID }
                             ).ToList();


                var menutype = new SelectList(db.MenuType_T.Where(x => x.ShopID == ShopID), "MenuTypeID", "MenuType", "MenuTypeID");

                ViewBag.MenuTypeID = menutype.ToList();


                var smenus = (from sm in db.MenuSubItem_T
                              join smig in db.MenuSubItemGroup_T on sm.MenuSubItemGroupID equals smig.MenuSubItemGroupID
                              where smig.ShopID == ShopID
                              orderby smig.MenuSubItemGroupID
                              select new SubMenu { MenuSubItemGroupID = smig.MenuSubItemGroupID, ShopID = smig.ShopID, MenuSubItemGroup = smig.MenuSubItemGroup, MenuSubItem = sm.MenuSubItem, MultipleSelectable = sm.MultipleSelectable, Paid = sm.Paid, Amount = sm.Amount }
                              ).ToList();

                ViewBag.Submenus = smenus.ToList();

                string groupnamex = "";
                string flag = "no";
                int looper = 0;

                foreach (var mitem in menus)
                {
                    var smenugroupid = db.Database.SqlQuery<MenuSubMenu>(@"select * from [dbo].[Menu-MenuSubItemGroup_T] mmsig
                        join [dbo].[MenuSubItemGroup_T] msig on mmsig.menusubitemgroupid = msig.menusubitemgroupid
                        join  [dbo].[MenuSubItem_T] msi on mmsig.menusubitemgroupid = msi.menusubitemgroupid
                        where menuid=" + mitem.MenuID + " order by msig.menusubitemgroupid");
                    looper = 0;
                    foreach (var msitem in smenugroupid)
                    {
                        if (flag != msitem.MenuSubItemGroup)
                        {
                            if (looper == 0)
                            {
                                groupnamex += msitem.MenuSubItemGroup;
                            }
                            else
                            {
                                groupnamex += ", " + msitem.MenuSubItemGroup;
                            }

                        }
                        flag = msitem.MenuSubItemGroup;
                        looper++;
                    }
                    mitem.MenuSubItemGroup = groupnamex;
                    groupnamex = "";
                    flag = "no";
                }


                ViewBag.Menus = menus.ToList();



                db.Dispose();
                SqlConnection.ClearAllPools();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Menu(Menu_T menu_t, HttpPostedFileBase pic)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                
                db.Menu_T.Add(menu_t);
                db.SaveChanges();



                if (pic != null && pic.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(pic.FileName);

                    var path = Path.Combine(Server.MapPath("~/shoplogos"), "menupic-" + menu_t.ShopID + "-" + menu_t.MenuID + ".jpg");
                    pic.SaveAs(path);
                }


                db.Dispose();
                SqlConnection.ClearAllPools();
                return RedirectToAction("Menu", "Menu");

            }

        }


        [HttpPost]
        public ActionResult MenuEdit(Menu_T menu_t, HttpPostedFileBase pic)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Entry(menu_t).State = EntityState.Modified;
                db.SaveChanges();

                if (pic != null && pic.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(pic.FileName);

                    var path = Path.Combine(Server.MapPath("~/shoplogos"), "menupic-" + menu_t.ShopID + "-" + menu_t.MenuID + ".jpg");
                    pic.SaveAs(path);
                }



                SqlConnection.ClearAllPools();
            }
            return RedirectToAction("Menu", "Menu");
        }

        [HttpPost]
        public ActionResult MenuDelete(FormCollection col)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                Menu_T menu_t = db.Menu_T.Find(Convert.ToInt32(col["MenuID"]));
                db.Menu_T.Remove(menu_t);
                db.SaveChanges();
            }
            return RedirectToAction("Menu", "Menu");

        }

        [HttpPost]
        public ActionResult SubMenuDelete(FormCollection col)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("Delete from [Menu-MenuSubItemGroup_T] where MenuID=" + col["MenuID"]);
            }
            return RedirectToAction("Menu", "Menu");

        }

        [HttpPost]
        public ActionResult SubMenuItemDelete(FormCollection col)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("Delete from [MenuSubItem_T] where MenuSubItemID=" + col["MenuSubItemID"]);
            }
            return RedirectToAction("SubMenu");

        }

        [HttpPost]
        public ActionResult SubMenuItemEdit(FormCollection col)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("update menusubitem_t set menusubitem='" + col["MenuSubItem"] + "', multipleselectable=" + col["multipleselectable"] + ",paid=" + col["paid"] + ",amount=" + col["amount"] + " where  MenuSubItemID=" + col["MenuSubItemID"]);
            }
            return RedirectToAction("SubMenu", "Menu");

        }


        public ActionResult Submenu()
        {
            if (Session["LoggedIn"] == null) { return RedirectToAction("Login", "Menu"); }

            ViewBag.menuno = 6;
            ViewBag.submenuno = 3;


            int ShopID = Convert.ToInt32(Session["CurrentShopID"]);

            ViewBag.ShopID = ShopID;




            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.shopname = db.Shop_T.Where(x => x.ShopID == ShopID).Select(x => x.ShopName).SingleOrDefault();

                var smenus = (from sm in db.MenuSubItem_T
                              join smig in db.MenuSubItemGroup_T on sm.MenuSubItemGroupID equals smig.MenuSubItemGroupID
                              where smig.ShopID == ShopID
                              orderby smig.MenuSubItemGroupID
                              select new SubMenu { MenuSubItemGroupID = smig.MenuSubItemGroupID, ShopID = smig.ShopID, MenuSubItemGroup = smig.MenuSubItemGroup, MenuSubItem = sm.MenuSubItem, MultipleSelectable = sm.MultipleSelectable, Paid = sm.Paid, Amount = sm.Amount, MenuSubItemID = sm.MenuSubItemID }
                              ).ToList();


                ViewBag.Submenus = smenus.ToList();


                var menusubitem = new SelectList(db.MenuSubItemGroup_T.Where(x => x.ShopID == ShopID), "MenuSubItemGroupID", "MenuSubItemGroup", "MenuSubItemGroupID");

                ViewBag.MenuSubItemGroupID = menusubitem.ToList();




                db.Dispose();
                SqlConnection.ClearAllPools();
            }
            return View();
        }

        [HttpPost]
        public ActionResult SubMenuGroup(FormCollection col)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                db.Database.ExecuteSqlCommand("insert into [MenuSubItemGroup_T] (ShopID,MenuSubItemGroup) values(" + col["ShopID"] + ",'" + col["SubMenuGroup"] + "')");

                db.Dispose();
                SqlConnection.ClearAllPools();
            }

            return RedirectToAction("SubMenu", "Menu");
        }

        [HttpPost]
        public ActionResult SubMenuItem(FormCollection col)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {

                db.Database.ExecuteSqlCommand("insert into[MenuSubItem_T](MenuSubItemGroupID, MenuSubItem, MultipleSelectable, Paid, Amount) values(" + col["MenuSubItemGroupID"] + ",'" + col["SubMenu"] + "'," + col["msel"] + ", " + col["priced"] + ", '" + col["Amount"] + "')");

                db.Dispose();
                SqlConnection.ClearAllPools();
            }

            return RedirectToAction("SubMenu", "Menu");
        }


    }
}
