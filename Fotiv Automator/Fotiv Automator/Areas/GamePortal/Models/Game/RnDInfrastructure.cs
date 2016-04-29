using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class RnDInfrastructure
    {
        public DB_civilization_rnd_infrastructure Info;

        public InfrastructureUpgrade BeingBuilt;
        public CivilizationInfrastructure BuildingAt;

        public Planet Planet;
        public Civilization Owner;

        public RnDInfrastructure(DB_civilization_rnd_infrastructure info, Civilization owner)
        {
            Info = info;
            Owner = owner;
        }
    }
}
