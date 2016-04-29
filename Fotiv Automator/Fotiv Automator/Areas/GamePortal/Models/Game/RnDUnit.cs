using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class RnDUnit
    {
        public DB_civilization_rnd_units Info;

        public Unit BeingBuilt;
        public CivilizationInfrastructure BuildingAt;
        public Civilization Owner;

        public RnDUnit(DB_civilization_rnd_units info, Civilization owner)
        {
            Info = info;
            Owner = owner;
        }
    }
}
