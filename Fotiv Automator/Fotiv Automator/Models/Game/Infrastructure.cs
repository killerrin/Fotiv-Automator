using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class Infrastructure
    {
        public DB_civilization_infrastructure Info;

        public Planet Planet;
        public DB_infrastructure InfrastructureInfo;
        public List<DB_infrastructure_upgrades> UpgradesInfo = new List<DB_infrastructure_upgrades>();

        public Infrastructure(DB_civilization_infrastructure dbCivilizationInfrastructure)
        {
            Info = dbCivilizationInfrastructure;
        }
    }
}
