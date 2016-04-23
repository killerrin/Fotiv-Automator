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
                //.Include("~/content/styles/bootstrap.css")
                //.Include("~/content/styles/bootstrap-theme.css")
                .Include("~/content/styles/bootstrap-theme-slate.css")
                .Include("~/content/styles/bootstrap-sortable.css")
                //.Include("~/content/styles/bootstrap-panel-tab.css")
                .Include("~/content/styles/site.css"));

            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/scripts/jquery/jquery-2.2.1.js")
                .Include("~/scripts/jquery/jquery.timeago.js")
                .Include("~/scripts/jquery/jquery.validate.js")
                .Include("~/scripts/jquery/jquery.validate.unobtrusive.js")
                .Include("~/scripts/moment.min.js")
                .Include("~/scripts/bootstrap/bootstrap.js")
                .Include("~/scripts/bootstrap/bootstrap-sortable.js")
                .Include("~/scripts/hexagon.js")
                .Include("~/scripts/ColorPicker/jQueryColorPicker.min.js")
                .Include("~/scripts/forms.js")
                .Include("~/scripts/frontend.js"));
        }
    }
}
