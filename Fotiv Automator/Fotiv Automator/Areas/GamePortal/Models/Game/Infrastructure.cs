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
        public int InfrastructureID { get { return InfrastructureInfo.Infrastructure.id; } }
        public bool IsBuilt { get { return CivilizationInfo.build_percentage >= 100; } }

        public DB_civilization_infrastructure CivilizationInfo;
        public Civilization Owner { get; set; } 

        public Planet Planet;
        public InfrastructureUpgrade InfrastructureInfo;

        public Infrastructure(DB_civilization_infrastructure dbCivilizationInfrastructure, Civilization owner)
        {
            CivilizationInfo = dbCivilizationInfrastructure;
            Owner = owner;
        }
    }
}
