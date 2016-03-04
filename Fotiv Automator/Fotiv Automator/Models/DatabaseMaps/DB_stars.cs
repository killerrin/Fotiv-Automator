using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_stars
    {
        public virtual int id { get; set; }
        public virtual int starsystem_id { get; set; }
        public virtual string name { get; set; }
        public virtual string age { get; set; }
        public virtual string radiation_level { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
