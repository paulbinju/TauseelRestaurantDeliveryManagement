using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauseelRestaurant.Models;

namespace TauseelRestaurant.Controllers
{
    public class appController : Controller
    {

        public ActionResult cuisines()
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var cuisine = (from c in db.Cuisine_T
                               where c.CuisineID != 15 && c.CuisineID != 33
                               orderby c.Cuisine
                               select new ShopCuisines { CuisineID = c.CuisineID, Cuisine = c.Cuisine }
                                  ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = cuisine,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult cuisine(int id)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var cuisine = (from sc in db.ShopCuisine_T
                               join c in db.Cuisine_T on sc.CuisineID equals c.CuisineID
                               where sc.ShopID == id
                               select new ShopCuisines { ShopID = sc.ShopID, CuisineID = c.CuisineID, Cuisine = c.Cuisine }
                                     ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = cuisine,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult shoptimings(int id)
        {

            string dayofweek = DateTime.UtcNow.ToString("ddd");
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var shoptiming = (from st in db.ShopTiming_T
                                  where st.ShopID == id && st.Day == dayofweek
                                  select new ShopTimings { ShopID = st.ShopID, Day = st.Day, StartTime = st.StartTime, EndTime = st.EndTime }
                                     ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = shoptiming,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        
        public ActionResult shopopenclose(int id)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                DateTime time = DateTime.UtcNow.AddHours(3);
                string today = time.DayOfWeek.ToString().Substring(0, 3);

                string status = "";

                var shoptiming = (from st in db.ShopTiming_T
                                  where st.ShopID == id && st.Day == today
                                  select new ShopTimings { ShopID = st.ShopID, Day = st.Day, StartTime = st.StartTime, EndTime = st.EndTime }
                                     ).SingleOrDefault();
                int shophouropen = DateTime.ParseExact(shoptiming.StartTime.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).Hour;
                int shophourclose = DateTime.ParseExact(shoptiming.EndTime.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).Hour;
                if (time.Hour >= shophouropen && time.Hour <= shophourclose)
                {
                    status = "open";
                }
                else
                {
                    status = "closed";
                }
                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = status,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult locations()
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var location = (from l in db.Location_T
                                orderby l.Location
                                select new ShopLocations { LocationID = l.LocationID, Location = l.Location }
                                  ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = location,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult location(int id)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var location = (from sl in db.ShopLocation_T
                                join l in db.Location_T on sl.LocationID equals l.LocationID
                                where sl.ShopID == id
                                select new ShopLocations { ShopID = sl.ShopID, LocationID = l.LocationID, Location = l.Location }
                                    ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = location,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult shopmenutypes(int id)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var menutypes = (from mt in db.MenuType_T
                                 where mt.ShopID == id
                                 orderby mt.MenuTypeID
                                 select new Menus { MenuType = mt.MenuType, MenuTypeID = mt.MenuTypeID }
                                  ).ToList();


                return new JsonpResult
                {
                    Data = menutypes,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult shopmenu(int id)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var menus = (from m in db.Menu_T
                             join mt in db.MenuType_T on m.MenuTypeID equals mt.MenuTypeID
                             where m.ShopID == id
                             orderby m.MenuTypeID
                             select new Menus { MenuType = mt.MenuType, MenuTypeID = m.MenuTypeID, Menu = m.Menu, Description = m.Description, Rate = m.Rate, MenuID = m.MenuID, ShopID = m.ShopID }
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


                var menu = menus.ToList();


                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = menu,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult shopsubmenu(int id)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {

                var submenu = db.Database.SqlQuery<SubMenu>("select * from [dbo].[Menu-MenuSubItemGroup_T] mmsig join [dbo].[MenuSubItemGroup_T] msig on msig.MenuSubItemGroupID = mmsig.MenuSubitemGroupID join [dbo].[MenuSubItem_T] msi on msi.MenuSubItemGroupID = msig.MenuSubItemGroupID where shopid=" + id).ToList();
                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = submenu,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult mappedshopid(int id)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var MainShopID = (from s in db.ShopMapping_T
                                  where s.ShopID == id
                                  select s.MainShopID
                                   ).ToList();
                db.Dispose();
                SqlConnection.ClearAllPools();
                return new JsonpResult
                {
                    Data = MainShopID,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult shops(int cuisineid, int locationid)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                //var location = db.Location_T.Where(x => x.LocationID == locationid).Select(x => x.Location).ToList();
                //ViewBag.Location = location[0];

                //var allshops = db.Database.SqlQuery<Shop>("select * from Shop_T where shopid in (select ShopID from ShopCuisine_T where CuisineID=" + cuisineid + ") and shopid in (select ShopID from ShopLocation_T where LocationID=" + locationid + ") and active=1");

                //ViewBag.AllShops = allshops.ToList();


                //if (allshops.ToList().Count == 0)
                //{

                //    ViewBag.Message = "Sorry! No Restaurants available for your search criteria, try again";
                //}

                string sql = "";
                if (cuisineid == 0 && locationid == 0)
                {
                    sql = "select top 100  s.ShopID,s.ShopName,s.Logo,s.HeaderPic,s.MinOrder,s.DeliveryCharge,s.PhoneNo,s.HasOffers,s.Discount,s.DeliveryTime,s.AcceptsCash,s.AcceptsCard,case when s.CurrentStatus=0 then 'OPEN'  when s.CurrentStatus=2 then 'CLOSED'  when s.CurrentStatus=1 then 'BUSY' END as CurrentStatusText,c.Cuisine   from shop_T s join shopcuisine_T sc on s.ShopID = sc.ShopID join cuisine_T c on sc.CuisineID = c.CuisineID and active=1 order by s.shopid desc";
                }
                else if (cuisineid != 0 && locationid == 0)
                {
                    sql = "select s.ShopID,s.ShopName,s.Logo,s.HeaderPic,s.MinOrder,s.DeliveryCharge,s.PhoneNo,s.HasOffers,s.Discount,s.DeliveryTime,s.AcceptsCash,s.AcceptsCard,case when s.CurrentStatus=0 then 'OPEN'  when s.CurrentStatus=2 then 'CLOSED'  when s.CurrentStatus=1 then 'BUSY' END as CurrentStatusText,c.Cuisine  from shop_T s join shopcuisine_T sc on s.ShopID = sc.ShopID join cuisine_T c on sc.CuisineID = c.CuisineID where s.shopid in (select shopid from Shop_T where shopid in (select ShopID from ShopCuisine_T where CuisineID = " + cuisineid + ") and active = 1) order by s.ShopID desc";
                }
                else if (cuisineid == 0 && locationid != 0)
                {
                    sql = "select  s.ShopID,s.ShopName,s.Logo,s.HeaderPic,s.MinOrder,s.DeliveryCharge,s.PhoneNo,s.HasOffers,s.Discount,s.DeliveryTime,s.AcceptsCash,s.AcceptsCard,case when s.CurrentStatus=0 then 'OPEN'  when s.CurrentStatus=2 then 'CLOSED'  when s.CurrentStatus=1 then 'BUSY' END as CurrentStatusText,c.Cuisine from shop_T s join shopcuisine_T sc on s.ShopID = sc.ShopID join cuisine_T c on sc.CuisineID = c.CuisineID where s.shopid in (select ShopID from ShopLocation_T where LocationID = " + locationid + ") and active = 1 order by s.ShopID desc";
                }
                else if (cuisineid != 0 && locationid != 0)
                {
                    sql = "select  s.ShopID,s.ShopName,s.Logo,s.HeaderPic,s.MinOrder,s.DeliveryCharge,s.PhoneNo,s.HasOffers,s.Discount,s.DeliveryTime,s.AcceptsCash,s.AcceptsCard,case when s.CurrentStatus=0 then 'OPEN'  when s.CurrentStatus=2 then 'CLOSED'  when s.CurrentStatus=1 then 'BUSY' END as CurrentStatusText,c.Cuisine from shop_T s join shopcuisine_T sc on s.ShopID = sc.ShopID join cuisine_T c on sc.CuisineID = c.CuisineID where s.shopid in (select shopid from Shop_T where shopid in (select ShopID from ShopCuisine_T where CuisineID = " + cuisineid + ") and shopid in (select ShopID from ShopLocation_T where LocationID = " + locationid + ") and active = 1) order by s.ShopID desc";
                }
                var shopswithcuisines = db.Database.SqlQuery<Shop>(sql).ToList();


                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = shopswithcuisines,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult shopdetails(int id)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var shop = (from s in db.Shop_T
                            where s.ShopID == id && s.Active == true
                            select new Shop { ShopID = s.ShopID, ShopName = s.ShopName, HeaderPic = s.HeaderPic, MinOrder = s.MinOrder, DeliveryCharge = s.DeliveryCharge, Logo = s.Logo, CurrentStatus = s.CurrentStatus, PhoneNo = s.PhoneNo, Discount = s.Discount, DeliveryTime = s.DeliveryTime, AcceptsCash= s.AcceptsCash, AcceptsCard= s.AcceptsCard }
                                ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = shop,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult shoplocation(int id, int locationid)
        {

            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                var shop = (from s in db.ShopLocation_T
                            where s.ShopID == id && s.LocationID == locationid
                            select new ShopLocations { ShopID = s.ShopID, MinOrder = s.LocMinOrder, DeliveryCharge = s.LocDeliveryCharge, DeliveryTime = s.LocDeliveryTime }
                                ).ToList();

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = shop,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult placeorder(int shopid = 0, int customerid = 0, string deliveryadvice = "", string grossamount = "", string deliverycharge = "")
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                Shop_T shopx = db.Shop_T.Where(x => x.ShopID == shopid).SingleOrDefault();

                Order_T order = new Order_T();
                order.OrderDate = DateTime.UtcNow.AddHours(3);
                order.StoreID = shopid;
                order.CustomerID = customerid;
                order.CurrentStatusID = 1;
                order.DeliveryAdvice = deliveryadvice;
                order.GrossAmount = Convert.ToDecimal(grossamount);
                order.DeliveryCharge = Convert.ToDecimal(deliverycharge);
                order.Discount = shopx.Discount;
                db.Order_T.Add(order);
                db.SaveChanges();

               // var ord = db.Order_T.Where(x => x.OrderID == order.OrderID).ToList();
                List<Order_T> ordx = new List<Order_T>();
                ordx.Add(order);

                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = ordx.ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        public ActionResult placeorderdetail(int orderid, string menu, string description, decimal amount, int quantity, int position)
        {
            using (TauseelEatEntities db = new TauseelEatEntities())
            {
                OrderDetail_T orderdetail;

                orderdetail = new OrderDetail_T();
                orderdetail.OrderID = orderid;
                orderdetail.Menu = menu;
                orderdetail.Description = description;
                orderdetail.Amount = amount;
                orderdetail.Quantity = quantity;
                orderdetail.Position = position;
                db.OrderDetail_T.Add(orderdetail);
                db.SaveChanges();


                db.Dispose();
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = "OK",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        //public ActionResult placeorderdetailsingledata(string querystringer)
        //{
        //    using (TauseelEatEntities db = new TauseelEatEntities())
        //    {
        //        OrderDetail_T orderdetail;

        //        orderdetail = new OrderDetail_T();
        //        orderdetail.OrderID = orderid;
        //        orderdetail.Menu = menu;
        //        orderdetail.Description = description;
        //        orderdetail.Amount = amount;
        //        orderdetail.Quantity = quantity;
        //        db.OrderDetail_T.Add(orderdetail);
        //        db.SaveChanges();


        //        db.Dispose();
        //        SqlConnection.ClearAllPools();

        //        return new JsonpResult
        //        {
        //            Data = "OK",
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }
        //}
        public ActionResult customercount() {

            using (TMain db2 =  new TMain())
            {

                var count = db2.Database.SqlQuery<customercount>("select count(customerid)*2 as count from customer_T").ToList();

                //int counter = count.Count;
               
                SqlConnection.ClearAllPools();

                return new JsonpResult
                {
                    Data = count,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }


    }

    public class JsonpResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            string jsoncallback = (context.RouteData.Values["jsoncallback"] as string) ?? request["jsoncallback"];
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                if (string.IsNullOrEmpty(base.ContentType))
                {
                    base.ContentType = "application/x-javascript";
                }
                response.Write(string.Format("{0}((", jsoncallback));
            }
            base.ExecuteResult(context);
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                response.Write("))");
            }
        }
    }
}