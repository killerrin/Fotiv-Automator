using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_research
    {
        public virtual int id { get; set; }

        public virtual int rp_cost { get; set; }
        public virtual string name { get; set; }
        public virtual string description { get; set; }
        public virtual int attack_bonus { get; set; }
        public virtual int health_bonus { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
