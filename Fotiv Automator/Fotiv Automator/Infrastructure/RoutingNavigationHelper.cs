using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fotiv_Automator.Infrastructure
{
    public static class RoutingNavigationHelper
    {
        public static void Home() { HttpContext.Current.Response.RedirectToRoute("Home"); }

        public static void Login() { HttpContext.Current.Response.RedirectToRoute("Login"); }
        public static void Logout() { HttpContext.Current.Response.RedirectToRoute("Logout"); }
        public static void CreateAccount() { HttpContext.Current.Response.RedirectToRoute("CreateAccount"); }
        public static void AccountSettings() { HttpContext.Current.Response.RedirectToRoute("AccountSettings"); }

        public static void GamesIndex() { HttpContext.Current.Response.RedirectToRoute("Games"); }
        public static void GameNew() { HttpContext.Current.Response.RedirectToRoute("NewGame"); }
        public static void GameView(int _gameid) { HttpContext.Current.Response.RedirectToRoute("Game", new { gameID = _gameid }); }
        public static void GameEdit(int _gameid) { HttpContext.Current.Response.RedirectToRoute("GameSettings", new { gameID = _gameid }); }

    }
}
