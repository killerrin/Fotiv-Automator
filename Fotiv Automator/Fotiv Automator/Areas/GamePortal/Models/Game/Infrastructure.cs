using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Infrastructure
    {
        public DB_civilization_infrastructure Info;

        public Planet Planet;
        public InfrastructureUpgrade InfrastructureInfo;

        public Infrastructure(DB_civilization_infrastructure dbCivilizationInfrastructure)
        {
            Info = dbCivilizationInfrastructure;
        }
    }
}
