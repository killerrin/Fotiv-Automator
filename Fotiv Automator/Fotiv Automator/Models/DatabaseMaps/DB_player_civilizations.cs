using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_player_civilizations
    {
        public virtual int player_id { get; set; }
        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
    }
}
