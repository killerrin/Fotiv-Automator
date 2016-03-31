using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class CivilizationAssets
    {
        public readonly int CivilizationID;

        #region Research
        public List<Research> ResearchRaw = new List<Research>();
        public List<Research> IncompleteResearch = new List<Research>();
        public List<Research> CompletedResearch = new List<Research>();
        public int TotalResearchSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.research_slot)
                        total++;
                return total;
            }
        }
        #endregion

        #region Infrastructure
        public List<Infrastructure> InfrastructureRaw = new List<Infrastructure>();
        public List<Infrastructure> IncompleteInfrastructure = new List<Infrastructure>();
        public List<Infrastructure> CompletedInfrastructure = new List<Infrastructure>();
        public int TotalColonialDevelopmentSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.colonial_development_slot)
                        total++;
                return total;
            }
        }
        #endregion

        #region Ships
        public List<CivilizationShip> ShipsRaw;
        public List<CivilizationShip> IncompleteShips = new List<CivilizationShip>();
        public List<CivilizationShip> CompletedShips = new List<CivilizationShip>();
        public int TotalShipConstructionSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.ship_construction_slot)
                        total++;
                return total;
            }
        }
        #endregion

        public CivilizationAssets(int civilizationID)
        {
            CivilizationID = civilizationID;
        }

        public void SortCompletedIncomplete()
        {
            // Research
            IncompleteResearch = ResearchRaw
                .Where(x => x.CivilizationInfo.build_percentage < 100)
                .ToList();

            CompletedResearch = ResearchRaw
                .Where(x => x.CivilizationInfo.build_percentage >= 100)
                .ToList();

            // Infrastructure
            IncompleteInfrastructure = InfrastructureRaw
                .Where(x => x.CivilizationInfo.build_percentage < 100)
                .ToList();

            CompletedInfrastructure = InfrastructureRaw
                .Where(x => x.CivilizationInfo.build_percentage >= 100)
                .ToList();

            // Ships
            IncompleteShips = ShipsRaw
                .Where(x => x.CivilizationInfo.build_percentage < 100)
                .ToList();

            CompletedShips = ShipsRaw
                .Where(x => x.CivilizationInfo.build_percentage >= 100)
                .ToList();
        }
    }
}
