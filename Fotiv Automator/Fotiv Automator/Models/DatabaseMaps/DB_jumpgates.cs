using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_jumpgates
    {
        public virtual int id { get; set; }
        public virtual int civ_struct_id { get; set; }
        public virtual int from_system_id { get; set; }
        public virtual int to_system_id { get; set; }
        public virtual string gmnotes { get; set; }
    }
}
