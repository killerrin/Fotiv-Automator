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
        public List<DB_infrastructure> UpgradeInfrastructure = new List<DB_infrastructure>();

        public InfrastructureUpgrade() { }
        public InfrastructureUpgrade(DB_infrastructure infrastructure)
        {
            Infrastructure = infrastructure;
        }

        public bool IsUpgrade(int id)
        {
            foreach (var upgrade in Upgrades)
                if (id == upgrade.to_infra_id)
                    return true;
            return false;
        }
    }
}
