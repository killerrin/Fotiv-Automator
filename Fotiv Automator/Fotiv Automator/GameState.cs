using NHibernate.Linq;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fotiv_Automator.Models.Game;

namespace Fotiv_Automator
{
    public class GameState
    {
        private const string GameKey = "Fotiv_Automator.GameState.GameKey";
        public static Game Game
        {
            get { return HttpContext.Current.Items[GameKey] as Game; }
            set { HttpContext.Current.Items[GameKey] = value; }
        }

        private const string GameIDKey = "Fotiv_Automator.GameState.GameIDKey";
        public static int? GameID
        {
            get { return HttpContext.Current.Items[GameIDKey] as int?; }
            set { HttpContext.Current.Items[GameIDKey] = value; }
        }

        public static void Set(Game game)
        {
            Game = game;
            GameID = game.Info.id;
        }
        public static void Reset()
        {
            GameID = null;
            Game = null;
        }
    }
}
