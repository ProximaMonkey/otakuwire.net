using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Otakuwire.DomainModel;

namespace Otakuwire
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                                          // Route name
                "{controller}/{action}/{id}",                                       // URL with parameters.
                new { controller = "Home", action = "Index", id = "" }              // Parameter defaults
            );

            routes.MapRoute(
                "SEO",                                                              // Route name
                "{controller}/{action}/{id}/{seo}",                                 // URL with parameters, SEO will up completely optional and used for strings to provide Search Engine Optimization.
                new { controller = "Home", action = "Index", id = "", seo = "" }    // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            Session["User"] = new DomainModel.DataEntities.User();
        }
    }
}