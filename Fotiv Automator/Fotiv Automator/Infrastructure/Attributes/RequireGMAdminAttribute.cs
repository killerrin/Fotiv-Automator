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
    public class RequireGMAdminAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Debug.WriteLine($"{nameof(RequireGMAdminAttribute)}: {nameof(OnAuthorization)}");
            if (filterContext.Result == null)
            {
                User user = Auth.User;
                if (user == null)
                    HttpContext.Current.Response.RedirectToRoute("Login");

                Game game = GameState.Game;
                if (game == null)
                    HttpContext.Current.Response.RedirectToRoute("Home");

                if (!game.IsPlayerGM(user.ID) && !HttpContext.Current.User.IsInRole("Admin"))
                    HttpContext.Current.Response.RedirectToRoute("Game", new { gameID = game.Info.id });

                //string redirectURL = @"~/SessionRecovery/Index/" + sessionGuidCookieValue;
                //filterContext.Result = new RedirectResult(redirectURL);
            }
        }

        public static bool IsGMOrAdmin()
        {
            User user = Auth.User;
            if (user == null)
                return false;

            Game game = GameState.Game;
            if (game == null)
                return false;

            if (!game.IsPlayerGM(user.ID) && !HttpContext.Current.User.IsInRole("Admin"))
                return false;

            return true;
        }
    }
}
