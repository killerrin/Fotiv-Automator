using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_infrastructure
    {
        public virtual int id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int planet_id { get; set; }
        public virtual int struct_id { get; set; }

        public virtual string name { get; set; }

        public virtual int build_percentage { get; set; }
        public virtual int current_health { get; set; }

        public virtual bool can_upgrade { get; set; }
        public virtual bool is_military { get; set; }

        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
