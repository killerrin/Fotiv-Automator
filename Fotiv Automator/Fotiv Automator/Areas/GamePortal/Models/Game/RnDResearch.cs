using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class RnDResearch
    {
        public DB_civilization_rnd_research Info;

        public DB_research BeingResearched;
        public CivilizationInfrastructure BuildingAt;
        public Civilization Owner;

        public RnDResearch(DB_civilization_rnd_research info, Civilization owner)
        {
            Info = info;
            Owner = owner;
        }
    }
}
