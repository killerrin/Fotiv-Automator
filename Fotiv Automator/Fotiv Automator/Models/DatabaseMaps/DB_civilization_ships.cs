using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_ships
    {
        public virtual int id { get; set; }
        public virtual int? ship_battlegroup_id { get; set; }
        public virtual int ship_id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int starsystem_id { get; set; }

        public virtual int build_percentage { get; set; }
        public virtual int current_health { get; set; }
        public virtual bool command_and_control { get; set; }
        public virtual string notes { get; set; }
        public virtual string gnotes { get; set; }
    }
}
