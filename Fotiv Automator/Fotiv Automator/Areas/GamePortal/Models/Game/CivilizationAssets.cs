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
        public readonly Civilization Owner;

        #region Research
        public List<Research> ResearchRaw = new List<Research>();
        public List<Research> IncompleteResearch = new List<Research>();
        public List<Research> CompletedResearch = new List<Research>();
        public bool HasResearchSlots { get { return IncompleteResearch.Count < TotalResearchSlots; } }
        public int TotalResearchSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.research_slot && completed.CivilizationInfo.current_health > 0)
                        total++;
                return total;
            }
        }
        #endregion

        #region Infrastructure
        public List<Infrastructure> InfrastructureRaw = new List<Infrastructure>();
        public List<Infrastructure> IncompleteInfrastructure = new List<Infrastructure>();
        public List<Infrastructure> CompletedInfrastructure = new List<Infrastructure>();
        public bool HasColonialDevelopmentSlots { get { return IncompleteInfrastructure.Count < TotalColonialDevelopmentSlots; } }
        public int TotalColonialDevelopmentSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.colonial_development_slot && completed.CivilizationInfo.current_health > 0)
                        total++;
                return total;
            }
        }
        #endregion

        #region Ships
        public List<CivilizationShip> ShipsRaw;
        public List<CivilizationShip> IncompleteShips = new List<CivilizationShip>();
        public List<CivilizationShip> CompletedShips = new List<CivilizationShip>();
        public List<BattlegroupShip> BattlegroupShips = new List<BattlegroupShip>();
        public bool HasShipConstructionSlots { get { return IncompleteShips.Count < TotalShipConstructionSlots; } }
        public int TotalShipConstructionSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.ship_construction_slot && completed.CivilizationInfo.current_health > 0)
                        total++;
                return total;
            }
        }
        #endregion

        #region Units
        public List<CivilizationUnit> UnitsRaw;
        public List<CivilizationUnit> IncompleteUnits = new List<CivilizationUnit>();
        public List<CivilizationUnit> CompletedUnits = new List<CivilizationUnit>();
        public List<BattlegroupUnit> BattlegroupUnits = new List<BattlegroupUnit>();
        public bool HasUnitTrainingSlots { get { return IncompleteUnits.Count < TotalUnitTrainingSlots; } }
        public int TotalUnitTrainingSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.InfrastructureInfo.Infrastructure.unit_training_slot && completed.CivilizationInfo.current_health > 0)
                        total++;
                return total;
            }
        }
        #endregion

        public CivilizationAssets(Civilization civilization)
        {
            Owner = civilization;
            CivilizationID = civilization.ID;
        }

        #region Bonus Calculations
        public int CalculateScienceBuildRate(bool isMilitary)
        {
            int buildRate = 10;

            foreach (var research in CompletedResearch)
                if (research.ResearchInfo.apply_military == isMilitary)
                    buildRate += research.ResearchInfo.science_bonus;
            foreach (var infrastructure in CompletedInfrastructure)
                if (infrastructure.InfrastructureInfo.Infrastructure.is_military == isMilitary && infrastructure.CivilizationInfo.current_health > 0)
                    buildRate += infrastructure.InfrastructureInfo.Infrastructure.science_bonus;

            if (Owner?.CivilizationTrait1.apply_military == isMilitary)
                buildRate += Owner.CivilizationTrait1.science_bonus;
            if (Owner?.CivilizationTrait2.apply_military == isMilitary)
                buildRate += Owner.CivilizationTrait2.science_bonus;
            if (Owner?.CivilizationTrait3.apply_military == isMilitary)
                buildRate += Owner.CivilizationTrait3.science_bonus;

            return buildRate;
        }

        public int CalculateColonialDevelopmentBonus(bool isMilitary)
        {
            int bonus = 0;

            foreach (var research in CompletedResearch)
                if (research.ResearchInfo.apply_military == isMilitary)
                    bonus += research.ResearchInfo.colonial_development_bonus;
            foreach (var infrastructure in CompletedInfrastructure)
                if (infrastructure.InfrastructureInfo.Infrastructure.is_military == isMilitary && infrastructure.CivilizationInfo.current_health > 0)
                    bonus += infrastructure.InfrastructureInfo.Infrastructure.colonial_development_bonus;

            if (Owner?.CivilizationTrait1.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait1.colonial_development_bonus;
            if (Owner?.CivilizationTrait2.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait2.colonial_development_bonus;
            if (Owner?.CivilizationTrait3.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait3.colonial_development_bonus;

            return bonus;
        }

        public int CalculateShipConstructionBonus(bool isMilitary)
        {
            int bonus = 0;

            foreach (var research in CompletedResearch)
                if (research.ResearchInfo.apply_military == isMilitary)
                    bonus += research.ResearchInfo.ship_construction_bonus;
            foreach (var infrastructure in CompletedInfrastructure)
                if (infrastructure.InfrastructureInfo.Infrastructure.is_military == isMilitary && infrastructure.CivilizationInfo.current_health > 0)
                    bonus += infrastructure.InfrastructureInfo.Infrastructure.ship_construction_bonus;

            if (Owner?.CivilizationTrait1.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait1.ship_construction_bonus;
            if (Owner?.CivilizationTrait2.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait2.ship_construction_bonus;
            if (Owner?.CivilizationTrait3.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait3.ship_construction_bonus;

            return bonus;
        }

        public int CalculateUnitTrainingBonus(bool isMilitary)
        {
            int bonus = 0;

            foreach (var research in CompletedResearch)
                if (research.ResearchInfo.apply_military == isMilitary)
                    bonus += research.ResearchInfo.unit_training_bonus;
            foreach (var infrastructure in CompletedInfrastructure)
                if (infrastructure.InfrastructureInfo.Infrastructure.is_military == isMilitary && infrastructure.CivilizationInfo.current_health > 0)
                    bonus += infrastructure.InfrastructureInfo.Infrastructure.unit_training_bonus;

            if (Owner?.CivilizationTrait1.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait1.unit_training_bonus;
            if (Owner?.CivilizationTrait2.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait2.unit_training_bonus;
            if (Owner?.CivilizationTrait3.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait3.unit_training_bonus;

            return bonus;
        }
        #endregion

        public int CalculateIncomePerTurn()
        {
            int income = 0;

            foreach (var infrastructure in CompletedInfrastructure)
                if (infrastructure.CivilizationInfo.current_health > 0)
                    income += infrastructure.InfrastructureInfo.Infrastructure.rp_bonus;

            int tradeBonus = 0;
            if (Owner?.CivilizationTrait1.trade_bonus != null)
                tradeBonus += Owner.CivilizationTrait1.trade_bonus;
            if (Owner?.CivilizationTrait2.trade_bonus != null)
                tradeBonus += Owner.CivilizationTrait2.trade_bonus;
            if (Owner?.CivilizationTrait3.trade_bonus != null)
                tradeBonus += Owner.CivilizationTrait3.trade_bonus;

            return income;
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

            BattlegroupShips = new List<BattlegroupShip>();
            var civilizationShipBattlegroups = Owner.ThisGame.GameStatistics.ShipBattlegroupsRaw
                .Where(x => x.civilization_id == CivilizationID)
                .ToList();
            foreach (var bg in civilizationShipBattlegroups)
                BattlegroupShips.Add(new BattlegroupShip(bg));

            foreach (var ship in CompletedShips)
            {
                if (ship.CivilizationInfo.ship_battlegroup_id == null) continue;

                var battlegroup = BattlegroupShips.Where(x => x.ID == ship.CivilizationInfo.ship_battlegroup_id).FirstOrDefault();
                if (battlegroup == null)
                    continue;
                battlegroup.Ships.Add(ship);
            }

            // Units
            IncompleteUnits = UnitsRaw
                .Where(x => x.CivilizationInfo.build_percentage < 100)
                .ToList();

            CompletedUnits = UnitsRaw
                .Where(x => x.CivilizationInfo.build_percentage >= 100)
                .ToList();

            BattlegroupUnits = new List<BattlegroupUnit>();
            var civilizationUnitBattlegroups = Owner.ThisGame.GameStatistics.UnitBattlegroupsRaw
                .Where(x => x.civilization_id == CivilizationID)
                .ToList();
            foreach (var bg in civilizationUnitBattlegroups)
                BattlegroupUnits.Add(new BattlegroupUnit(bg));

            foreach (var unit in CompletedUnits)
            {
                if (unit.CivilizationInfo.unit_battlegroup_id == null) continue;

                var battlegroup = BattlegroupUnits.Where(x => x.ID == unit.CivilizationInfo.unit_battlegroup_id).FirstOrDefault();
                if (battlegroup == null)
                    continue;
                battlegroup.Units.Add(unit);
            }
        }
    }
}
