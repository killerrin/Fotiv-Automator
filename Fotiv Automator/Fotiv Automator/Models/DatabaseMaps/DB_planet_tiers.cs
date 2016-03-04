using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_planet_tiers
    {
        public virtual int id { get; set; }
        public virtual string name { get; set; }
        public virtual int build_rate { get; set; }
    }
}
