using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class InfrastructureUpgrade
    {
        public DB_infrastructure Infrastructure;
        public List<DB_infrastructure_upgrades> Upgrades = new List<DB_infrastructure_upgrades>();

        public InfrastructureUpgrade() { }
        public InfrastructureUpgrade(DB_infrastructure infrastructure)
        {
            Infrastructure = infrastructure;
        }
    }
}
