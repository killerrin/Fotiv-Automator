using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class HomeIndex
    {
        public DB_users User { get; set; }
        public IEnumerable<Tuple<DB_games, DB_game_users>> Games { get; set; } = new List<Tuple<DB_games, DB_game_users>>();
    }
}



