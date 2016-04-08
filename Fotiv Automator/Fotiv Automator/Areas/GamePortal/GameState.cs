using NHibernate.Linq;
using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;

namespace Fotiv_Automator.Areas.GamePortal
{
    public static class GameState
    {
        private const string GameKey = "Fotiv_Automator.GameState.GameKey";
        public static Game Game
        {
            get { return HttpContext.Current.Session[GameKey] as Game; }
        }

        private const string GameIDKey = "Fotiv_Automator.GameState.GameIDKey";
        public static int? GameID
        {
            get { return HttpContext.Current.Session[GameIDKey] as int?; }
        }


        public static void Set(Game game)
        {
            //Clear();

            Debug.WriteLine(string.Format("GameState: Set"));
            HttpContext.Current.Session[GameKey] = game;
            HttpContext.Current.Session[GameIDKey] = game.Info.id;
        }
        public static void Clear()
        {
            Debug.WriteLine(string.Format("GameState: Clear"));
            HttpContext.Current.Session[GameKey] = null;
            HttpContext.Current.Session[GameIDKey] = null;
        }

        public static Game QueryGame(int? gameID = null, bool useCache = false)
        {
            Debug.WriteLine(string.Format("GameState: Query Game id={0}", gameID));

            if (gameID == null)
            {
                if (GameID != null)
                {
                    if (useCache) return Game;
                    gameID = GameID.Value;
                }
                else return null;
            }

            DB_games db_game = Database.Session.Query<DB_games>()
                .Where(x => x.id == gameID)
                .First();

            Game game = new Game(db_game);
            GameState.Set(game);

            return game;
        }
    }
}
