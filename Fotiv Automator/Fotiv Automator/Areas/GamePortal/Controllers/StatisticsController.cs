using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class StatisticsController : Controller
    {
        // GET: GamePortal/Statistics
        [HttpGet]
        public ActionResult Index(int gameID)
        {
            Debug.WriteLine(string.Format("GET: Statistics Controller: Index"));
            Game game = GameState.Game;
            game.QueryGameStatistics();

            SafeUser user = Auth.User;
            GamePlayer player = game.GetPlayer(user.ID);
            if (player == null)
            {
                player = new GamePlayer
                {
                    User = user,
                    GameUserInfo = new DB_game_users
                    {
                        user_id = user.ID,
                        game_id = game.Info.id,
                        is_gm = false
                    }
                };
            }

            return View(new IndexStatistics
            {
                User = player,
                Statistics = game.GameStatistics
            });
        }
    }
}