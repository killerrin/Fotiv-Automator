using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Fotiv_Automator.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/site.css"));
            
            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/scripts/jquery-2.2.1.js")
                .Include("~/scripts/jquery.timeago.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/frontend.js"));
        }
    }
}
