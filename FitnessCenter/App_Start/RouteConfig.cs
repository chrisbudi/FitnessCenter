using System.Web.Mvc;
using System.Web.Routing;

namespace FitnessCenter
{
    public class RouteConfig
    {
        public static void RegisterRoutes(AreaRegistrationContext context)
        {
            //add routes
            context.MapRoute(
                "Dashboard_default",
                "Dashboard/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //            context.MapRoute("Dashboard_default", "{controller}/{action}/{id}",
            //                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //                );

            context.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*(CrystalImageHandler).*" });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Default_Print", "CrystalReport/LoadReport/{ReportModel}",
            //    new { controller = "CrystalReport", action = "LoadReport",
            //        ReportModel = UrlParameter.Optional }
            //    );

            routes.MapRoute("Default_PostAction", "Calon/{salesAction}/{salesStatus}/{id}",
                new { controller = "Sales", action = "", id = UrlParameter.Optional }
                );

            //routes.MapRoute("Default_Capture", "Image/CaptureImage/{id}",
            //    new { controller = "Home", action = "CaptureImage", id = UrlParameter.Optional }
            //    );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}