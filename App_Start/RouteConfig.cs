using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TauseelRestaurant
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "appplaceorder",
            url: "app/placeorder/{shopid}/{customerid}/{deliveryadvice}/{grossamount}/{deliverycharge}",
            defaults: new { controller = "app", action = "placeorder", shopid = UrlParameter.Optional, customerid = UrlParameter.Optional, deliveryadvice = UrlParameter.Optional, grossamount = UrlParameter.Optional, deliverycharge = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "appplaceorderdetail",
            url: "app/placeorderdetail/{orderid}/{menu}/{description}/{amount}/{quantity}/{position}",
            defaults: new { controller = "app", action = "placeorderdetail", orderid = UrlParameter.Optional, menu = UrlParameter.Optional, description = UrlParameter.Optional, amount = UrlParameter.Optional, quantity = UrlParameter.Optional, position = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "appshops",
            url: "app/shops/{cuisineid}/{locationid}",
            defaults: new { controller = "app", action = "shops", cuisinieid = UrlParameter.Optional, shopid = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "appshoplocation",
            url: "app/shoplocation/{id}/{locationid}",
            defaults: new { controller = "app", action = "shoplocation", id = UrlParameter.Optional, locationid = UrlParameter.Optional }
            );

            


            routes.MapRoute(
               name: "DeleteConfirmed",
               url: "Rating/DeleteConfirmed/{id}",
               defaults: new { controller = "Rating", action = "DeleteConfirmed", id = UrlParameter.Optional }
             );




            routes.MapRoute(
                       name: "Delivery",
                       url: "Delivery",
                       defaults: new { controller = "Delivery", action = "Home", id = UrlParameter.Optional }
                   );


            routes.MapRoute(
           name: "DOrders",
           url: "Orders",
           defaults: new { controller = "Delivery", action = "Orders", id = UrlParameter.Optional }
       );


            routes.MapRoute(
                name: "DeliveryMenuEdit",
                url: "Delivery/MenuEdit",
                defaults: new { controller = "Delivery", action = "MenuEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                   name: "Report",
                   url: "Report",
                   defaults: new { controller = "Report", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                   name: "ReportSummary",
                   url: "ReportSummary",
                   defaults: new { controller = "OrderDetail", action = "ReportSummary", id = UrlParameter.Optional }
               );

            routes.MapRoute(
               name: "ReportDetails",
               url: "ReportDetails",
               defaults: new { controller = "OrderDetail", action = "ReportDetails", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                   name: "BillCreate",
                   url: "Order/BillCreate",
                   defaults: new { controller = "Order", action = "BillCreate", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                 name: "BillUpload",
                 url: "Order/BillUpload",
                 defaults: new { controller = "Order", action = "BillUpload", id = UrlParameter.Optional }
             );





            routes.MapRoute(
           name: "Login",
           url: "Login",
           defaults: new { controller = "Default", action = "Login", id = UrlParameter.Optional }
       );


            routes.MapRoute(
           name: "Logout",
           url: "Logout",
           defaults: new { controller = "Default", action = "Logout", id = UrlParameter.Optional }
       );
            routes.MapRoute(
               name: "OrderDetail",
               url: "OrderDetail/{orderid}",
               defaults: new { controller = "OrderDetail", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "DeliveryOrderPrint",
               url: "Delivery/OrderPrint/{orderid}",
               defaults: new { controller = "Delivery", action = "OrderPrint", orderid = UrlParameter.Optional }
            );


            routes.MapRoute(
               name: "OrderPrint",
               url: "OrderPrint/{orderid}",
               defaults: new { controller = "Order", action = "OrderPrint", orderid = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Order",
               url: "Order/{type}",
               defaults: new { controller = "Order", action = "Index", type = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "OrderCancelled",
               url: "OrderCancelled",
               defaults: new { controller = "Order", action = "OrderCancelled" }
            );


            routes.MapRoute(
               name: "OrderProcessed",
               url: "OrderProcessed/{type}",
               defaults: new { controller = "Order", action = "OrderProcessed", type = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Menu",
              url: "Menu/{id}",
              defaults: new { controller = "Menu", action = "Menu", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "SubMenu",
             url: "SubMenu/{id}",
             defaults: new { controller = "Menu", action = "SubMenu", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "SubMenuGroup",
             url: "SubMenuGroup",
             defaults: new { controller = "Menu", action = "SubMenuGroup" }
            );
            routes.MapRoute(
            name: "SubMenuAdd",
            url: "SubMenuAdd",
            defaults: new { controller = "Menu", action = "SubMenuAdd" }
           );
            routes.MapRoute(
            name: "MenuDelete",
            url: "MenuDelete",
            defaults: new { controller = "Menu", action = "MenuDelete" }
            );

            routes.MapRoute(
            name: "SubMenuDelete",
            url: "SubMenuDelete",
            defaults: new { controller = "Menu", action = "SubMenuDelete" }
            );
            routes.MapRoute(
             name: "SubMenuItem",
             url: "SubMenuItem",
             defaults: new { controller = "Menu", action = "SubMenuItem" }
            );
            routes.MapRoute(
             name: "SubMenuItemEdit",
             url: "SubMenuItemEdit",
             defaults: new { controller = "Menu", action = "SubMenuItemEdit" }
            );

             routes.MapRoute(
             name: "SubMenuItemDelete",
             url: "SubMenuItemDelete",
             defaults: new { controller = "Menu", action = "SubMenuItemDelete" }
            );

            

            routes.MapRoute(
              name: "Filterx",
              url: "Filterx",
              defaults: new { controller = "Menu", action = "Filterx", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                  name: "MenuEdit",
                  url: "MenuEdit",
                  defaults: new { controller = "Menu", action = "MenuEdit" }
              );

            routes.MapRoute(
                   name: "AdminPanel",
                   url: "AdminPanel/{action}/{id}",
                   defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
            name: "RestaurantList",
            url: "RestaurantList",
            defaults: new { controller = "Website", action = "RestaurantList" }
            );
            routes.MapRoute(
                   name: "thankyou",
                   url: "thankyou",
                   defaults: new { controller = "Website", action = "Thankyou" }
                   );

            routes.MapRoute(
                  name: "Restaurant",
                  url: "Restaurant/{id}",
                  defaults: new { controller = "Website", action = "Restaurant", id = UrlParameter.Optional }
                  );

            routes.MapRoute(
       name: "Checkout",
       url: "Checkout/{id}",
       defaults: new { controller = "Website", action = "Checkout", id = UrlParameter.Optional }
       );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}