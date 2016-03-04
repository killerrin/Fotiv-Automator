using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_game_players
    {
        public virtual int player_id { get; set; }
        public virtual int game_id { get; set; }
        public virtual bool is_gm { get; set; }
    }
}
