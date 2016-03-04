using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_planets
    {
        public virtual int id { get; set; }
        public virtual int star_id { get; set; }
        public virtual int? planet_tier_id { get; set; }
        public virtual string name { get; set; }
        public virtual int resources { get; set; }
        public virtual bool supports_colonies { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
