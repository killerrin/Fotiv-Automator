using NHibernate.Linq;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fotiv_Automator.Models.Game;
using System.Diagnostics;

namespace Fotiv_Automator
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

        public static Game QueryGame(int id)
        {
            Debug.WriteLine(string.Format("GameState: Query Game id={0}", id));

            DB_games db_game = Database.Session.Query<DB_games>()
                .Where(x => x.id == id)
                .First();

            Game game = new Game(db_game);
            GameState.Set(game);

            return game;
        }
    }
}
