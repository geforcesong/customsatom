using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;

namespace CustomsAtomMobileSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            //this.AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        //void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        //{
        //    var id = Context.User.Identity as FormsIdentity;
        //    if (id != null && id.IsAuthenticated)
        //    {
        //        var roles = id.Ticket.UserData.Split(',');
        //        Context.User = new GenericPrincipal(id, roles);
        //    }
        //}

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes); 
        }
    }
}