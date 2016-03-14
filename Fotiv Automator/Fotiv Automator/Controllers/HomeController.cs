using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.ViewModels;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            DB_users user = Auth.User;
            if (user == null)
                return RedirectToRoute("login");

            var dbGames = Database.Session.Query<DB_games>().ToList();
            var dbGameUsers = Database.Session.Query<DB_game_users>()
                .Where(x => x.user_id == user.id)
                .ToList();

            List<Tuple<DB_games, DB_game_users>> games = new List<Tuple<DB_games, DB_game_users>>();
            foreach (var dbGame in dbGames)
                foreach (var dbGameUser in dbGameUsers)
                    if (dbGame.id == dbGameUser.game_id)
                        games.Add(new Tuple<DB_games, DB_game_users>(dbGame, dbGameUser));

            return View(new HomeIndex
            {
                User = user,
                Games = games
            });
        }
    }
}