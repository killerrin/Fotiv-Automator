using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_infrastructure_upgrades
    {
        public virtual int from_infra_id { get; set; }
        public virtual int to_infra_id { get; set; }
    }
}
