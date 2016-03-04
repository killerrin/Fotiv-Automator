using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_ships
    {
        public int id { get; set; }
        public int? ship_rate_id { get; set; }
        public int rp_cost { get; set; }
        public virtual int base_health { get; set; }
        public virtual int base_attack { get; set; }
        public virtual int maximum_fighters { get; set; }
        public virtual int num_build { get; set; }
        public virtual int gmnotes { get; set; }
    }
}
