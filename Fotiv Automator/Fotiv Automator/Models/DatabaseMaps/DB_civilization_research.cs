using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_research
    {
        public virtual int civilization_id { get; set; }
        public virtual int research_id { get; set; }
        public virtual int build_percentage { get; set; }
    }
}
