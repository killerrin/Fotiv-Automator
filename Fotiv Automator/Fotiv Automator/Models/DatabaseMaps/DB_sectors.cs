using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_sectors
    {
        public virtual int id { get; set; }
        public virtual string name { get; set; }
        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
