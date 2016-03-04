using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_starsystems
    {
        public virtual int id { get; set; }
        public virtual int sector_id { get; set; }
        public virtual int hex_x { get; set; }
        public virtual int hex_y { get; set; }
    }
}
