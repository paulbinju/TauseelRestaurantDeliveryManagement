using TauseelRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TauseelRestaurant.Controllers
{
    public class WebsiteControllerold : Controller
    {
        //
        // GET: /Website/

        public ActionResult Index()
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                ViewBag.Cuisines = db.Cuisine_T.ToList();
                ViewBag.Locations = db.Location_T.ToList();

                db.Dispose();
            }


            return View();
        }

        [HttpPost]
        public ActionResult RestaurantList(FormCollection col)
        {
            Session["MenuCart"] = null;
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                int cuisineid = Convert.ToInt32(col["cuisine"]);
                int locationid = Convert.ToInt32(col["location"]);


                var location = db.Location_T.Where(x => x.LocationID == locationid).Select(x => x.Location).ToList();
                ViewBag.Location = location[0];

                var allshops = db.Database.SqlQuery<Shop>("select * from Shop_T where shopid in (select ShopID from ShopCuisine_T where CuisineID=" + cuisineid + ") and shopid in (select ShopID from ShopLocation_T where LocationID=" + locationid + ") and active=1");

                ViewBag.AllShops = allshops.ToList();


                if (allshops.ToList().Count == 0)
                {

                    ViewBag.Message = "Sorry! No Restaurants available for your search criteria, try again";
                }


                ViewBag.AllCuisines = db.Database.SqlQuery<ShopCuisines>(@"select * from shop_T s join shopcuisine_T sc on s.ShopID = sc.ShopID join cuisine_T c on sc.CuisineID = c.CuisineID where s.shopid in (select shopid from Shop_T where shopid in (select ShopID from ShopCuisine_T where CuisineID = " + cuisineid + ") and shopid in (select ShopID from ShopLocation_T where LocationID = " + locationid + ") and active = 1)").ToList();


                db.Dispose();
            }


            return View();
        }
        public ActionResult Restaurant(int id)
        {
            ViewBag.ShopID = id;

            using (TauseelEatEntities db = new TauseelEatEntities())
            {


                var shop = db.Shop_T.Where(x => x.ShopID == id).ToList();

                ViewBag.Shop = shop.ToList();


                ViewBag.Cuisines = (from sc in db.ShopCuisine_T
                                    join c in db.Cuisine_T on sc.CuisineID equals c.CuisineID
                                    where sc.ShopID == id
                                    select new ShopCuisines { ShopID = sc.ShopID, CuisineID = c.CuisineID, Cuisine = c.Cuisine }
                                     ).ToList();

                ViewBag.Locations = (from sl in db.ShopLocation_T
                                     join l in db.Location_T on sl.LocationID equals l.LocationID
                                     where sl.ShopID == id
                                     select new ShopLocations { ShopID = sl.ShopID, LocationID = l.LocationID, Location = l.Location }
                                    ).ToList();


                var menus = (from m in db.Menu_T
                             join mt in db.MenuType_T on m.MenuTypeID equals mt.MenuTypeID
                             where m.ShopID == id
                             orderby m.MenuTypeID
                             select new Menus { MenuType = mt.MenuType, MenuTypeID = m.MenuTypeID, Menu = m.Menu, Description = m.Description, Rate = m.Rate, MenuID = m.MenuID, ShopID = m.ShopID }
                                  ).ToList();

                var smenus = (from sm in db.MenuSubItem_T
                              join smig in db.MenuSubItemGroup_T on sm.MenuSubItemGroupID equals smig.MenuSubItemGroupID
                              where smig.ShopID == id
                              orderby smig.MenuSubItemGroupID
                              select new SubMenu { MenuSubItemID = sm.MenuSubItemID, MenuSubItemGroupID = smig.MenuSubItemGroupID, ShopID = smig.ShopID, MenuSubItemGroup = smig.MenuSubItemGroup, MenuSubItem = sm.MenuSubItem, MultipleSelectable = sm.MultipleSelectable, Paid = sm.Paid, Amount = sm.Amount }
                     ).ToList();



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
                                groupnamex += msitem.MenuSubItemGroupID;
                            }
                            else
                            {
                                groupnamex += ", " + msitem.MenuSubItemGroupID;
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






                ViewBag.Submenus = smenus.ToList();



                db.Dispose();
            }
            // Cart
            List<Menus> MenuCart;
            if (Session["MenuCart"] != null)
            {
                MenuCart = (List<Menus>)Session["MenuCart"];
                ViewBag.MenuCart = MenuCart.ToList();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add2Cart(FormCollection col)
        {
            List<Menus> MenuCart;
            if (Session["MenuCart"] == null)
            {
                MenuCart = new List<Menus>();
            }
            else
            {
                MenuCart = (List<Menus>)Session["MenuCart"];
            }

            string qty = "quant[" + col["MenuID"] + "]";
            Menus menuitem;

            // main item
            menuitem = new Menus();
            menuitem.MenuID = Convert.ToInt32(col["MenuID"]);
            menuitem.Menu = col["Menu"];
            string descri = col["Description"];
            if (col["groupids"] != null)
            {
                string[] temp2 = col["groupids"].Split(',');
                for (int i = 0; i < temp2.Length; i++)
                {
                    descri = " * " + col[temp2[0]];
                }
            }

            menuitem.Description = descri;
            menuitem.Rate = Convert.ToDouble(col["Rate"]);
            menuitem.Qty = Convert.ToInt32(col[qty]);
            MenuCart.Add(menuitem);

            //------------check box items

            if (col["chkitems"] != null)
            {
                string[] temp = col["chkitems"].Split(',');
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] chkitem = temp[i].Split('-');

                    menuitem = new Menus();
                    menuitem.MenuID = Convert.ToInt32(col["MenuID"]);
                    menuitem.Menu = chkitem[0];
                    menuitem.Description = "";
                    menuitem.Rate = Convert.ToDouble(chkitem[1]);
                    menuitem.Qty = Convert.ToInt32(col[qty]);
                    MenuCart.Add(menuitem);
                }
            }






            Session["MenuCart"] = MenuCart;
            return RedirectToAction("Restaurant/" + col["ShopID"]);
        }

        public ActionResult CheckOut(int id)
        {

            List<Menus> MenuCart;
            if (Session["MenuCart"] != null)
            {
                MenuCart = (List<Menus>)Session["MenuCart"];
                ViewBag.MenuCart = MenuCart.ToList();
            }
            else
            {
                return RedirectToAction("Index", "Website");
            }

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var shop = db.Shop_T.Where(x => x.ShopID == id).ToList();
                ViewBag.Shop = shop;
                
                db.Dispose();
            }


            return View();
        }

        [HttpPost]
        public ActionResult PlaceOrder(FormCollection col)
        {

            List<Menus> MenuCart;
            if (Session["MenuCart"] != null)
            {
                MenuCart = (List<Menus>)Session["MenuCart"];
                ViewBag.MenuCart = MenuCart.ToList();
            }
            else
            {
                return RedirectToAction("Index", "Website");
            }

            if (MenuCart != null)
            {
                double? amt = 0.0;
                double? total = 0.0;
                using (TauseelEatEntities db = new TauseelEatEntities())
                {
                    ResCustomer_T customer = new ResCustomer_T();
                    customer.Name = Convert.ToString(col["name"]);
                    customer.Email = Convert.ToString(col["email"]);
                    customer.Mobile = Convert.ToString(col["mobile"]);
                    customer.FlatVilla = Convert.ToString(col["flat"]);
                    customer.Building = Convert.ToString(col["building"]);
                    customer.Road = Convert.ToString(col["road"]);
                    customer.Block = Convert.ToString(col["block"]);
                    customer.Location = Convert.ToString(col["area"]);
                    customer.Landmark = Convert.ToString(col["landmark"]);
                    db.ResCustomer_T.Add(customer);
                    db.SaveChanges();


                    foreach (var cart in MenuCart)
                    {
                        amt = cart.Rate * cart.Qty;
                        total += amt;
                    }

                    Order_T order = new Order_T();
                    order.OrderDate = DateTime.Now;
                    order.StoreID = Convert.ToInt32(col["ShopID"]);
                    order.CustomerID = customer.CustomerID;

                    order.CurrentStatusID = 1;
                    order.DeliveryAdvice = Convert.ToString(col["specialrequest"]);

                    db.Order_T.Add(order);
                    db.SaveChanges();

                    OrderDetail_T orderdetail;
                    foreach (var cart in MenuCart)
                    {
                        orderdetail = new OrderDetail_T();
                        orderdetail.OrderID = order.OrderID;
                        orderdetail.Menu = cart.Menu;
                        orderdetail.Description = cart.Description;
                        orderdetail.Amount = Convert.ToDecimal(cart.Rate);
                        orderdetail.Quantity = cart.Qty;
                        db.OrderDetail_T.Add(orderdetail);
                        db.SaveChanges();
                    }





                    db.Dispose();
                }



            }










            Session["MenuCart"] = null;
            return RedirectToAction("Thankyou");
        }

        public ActionResult Thankyou()
        {


            return View();
        }

    }
}
