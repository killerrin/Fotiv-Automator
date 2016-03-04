using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_wormholes
    {
        public virtual int id { get; set; }
        public virtual int system_id_one { get; set; }
        public virtual int system_id_two { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
