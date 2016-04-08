using Fotiv_Automator.Areas.GamePortal;
using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Infrastructure.Attributes
{
    public class RequireUserLoggedInAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Debug.WriteLine($"{nameof(RequireGMAdminAttribute)}: {nameof(OnAuthorization)}");
            if (filterContext.Result == null)
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    HttpContext.Current.Response.RedirectToRoute("login");

                SafeUser user = Auth.User;
                if (user == null)
                    HttpContext.Current.Response.RedirectToRoute("Login");
            }
        }
    }
}
