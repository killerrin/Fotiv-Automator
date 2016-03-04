using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_infrastructure
    {
        public virtual int id { get; set; }

        public virtual int rp_cost { get; set; }
        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual bool is_colony { get; set; }
        public virtual bool is_military { get; set; }

        public virtual int base_health { get; set; }
        public virtual int base_attack { get; set; }
        public virtual int influence { get; set; }

        public virtual int rp_bonus { get; set; }
        public virtual int science_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }

        public virtual bool research_slot { get; set; }
        public virtual bool ship_construction_slot { get; set; }
        public virtual bool colonial_development_slot { get; set; }

        public virtual string gmnotes { get; set; }
    }
}
