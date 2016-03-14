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
        private const string GameKey = "Fotiv_Automator.GameState.UserKey";

        public static Game Game
        {
            get { return HttpContext.Current.Items[GameKey] as Game; }
            set { HttpContext.Current.Items[GameKey] = Game; }
        }
    }
}
