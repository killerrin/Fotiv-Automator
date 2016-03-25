using Fotiv_Automator.App_Start;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Fotiv_Automator
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.Configure();
        }

        protected void Application_BeginRequest()
        {
            //Debug.WriteLine("Application_BeginRequest");
            Database.OpenSession();
        }

        protected void Application_EndRequest()
        {
            Auth.UpdateUserActivity();
            Database.CloseSession();
            //Debug.WriteLine("Application_EndRequest");
        }
    }
}
