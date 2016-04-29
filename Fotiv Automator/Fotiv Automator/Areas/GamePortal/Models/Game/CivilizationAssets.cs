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
        public List<RnDResearch> IncompleteResearch = new List<RnDResearch>();
        public List<Research> CompletedResearch = new List<Research>();
        public bool HasResearchSlots { get { return IncompleteResearch.Count < TotalResearchSlots; } }
        public int TotalResearchSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.CivilizationInfo.current_health > 0)
                        total += completed.InfrastructureInfo.Infrastructure.research_slots;
                return total;
            }
        }
        #endregion

        #region Infrastructure
        public List<RnDInfrastructure> IncompleteInfrastructure = new List<RnDInfrastructure>();
        public List<CivilizationInfrastructure> CompletedInfrastructure = new List<CivilizationInfrastructure>();
        public bool HasColonialDevelopmentSlots { get { return IncompleteInfrastructure.Count < TotalColonialDevelopmentSlots; } }
        public int TotalColonialDevelopmentSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.CivilizationInfo.current_health > 0)
                        total += completed.InfrastructureInfo.Infrastructure.colonial_development_slots;
                return total;
            }
        }
        #endregion

        #region Units
        public List<Battlegroup> Battlegroups = new List<Battlegroup>();

        public List<RnDUnit> IncompleteUnitsRaw = new List<RnDUnit>();
        public List<RnDUnit> IncompleteSpaceUnits = new List<RnDUnit>();
        public List<RnDUnit> IncompleteGroundUnits = new List<RnDUnit>();

        public List<CivilizationUnit> CompletedUnitsRaw = new List<CivilizationUnit>();
        public List<CivilizationUnit> CompletedSpaceUnits = new List<CivilizationUnit>();
        public List<CivilizationUnit> CompletedGroundUnits = new List<CivilizationUnit>();
        public bool HasShipConstructionSlots { get { return IncompleteSpaceUnits.Count < TotalShipConstructionSlots; } }
        public bool HasUnitTrainingSlots { get { return IncompleteGroundUnits.Count < TotalUnitTrainingSlots; } }
        public int TotalShipConstructionSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.CivilizationInfo.current_health > 0)
                        total += completed.InfrastructureInfo.Infrastructure.ship_construction_slots;
                return total;
            }
        }
        public int TotalUnitTrainingSlots
        {
            get
            {
                int total = 0;
                foreach (var completed in CompletedInfrastructure)
                    if (completed.CivilizationInfo.current_health > 0)
                        total += completed.InfrastructureInfo.Infrastructure.unit_training_slots;
                return total;
            }
        }
        #endregion

        public CivilizationAssets(Civilization civilization)
        {
            Owner = civilization;
            CivilizationID = civilization.ID;
        }

        #region Calculations
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


        public int CalculateScienceBuildRate(bool isMilitary)
        {
            int buildRate = 10;

            foreach (var research in CompletedResearch)
                if (research.ResearchInfo.apply_military == isMilitary)
                    buildRate += research.ResearchInfo.science_bonus;

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

            if (Owner?.CivilizationTrait1.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait1.unit_training_bonus;
            if (Owner?.CivilizationTrait2.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait2.unit_training_bonus;
            if (Owner?.CivilizationTrait3.apply_military == isMilitary)
                bonus += Owner.CivilizationTrait3.unit_training_bonus;

            return bonus;
        }
        #endregion

        #region Per Turn Actions
        public void ProcessTurn()
        {
            int militaryResearchBuildRate = CalculateScienceBuildRate(true);
            int civilianResearchBuildRate = CalculateScienceBuildRate(false);
            foreach (var research in IncompleteResearch)
            {
                if (research.BuildingAt.CivilizationInfo.current_health <= 0) continue;
                int buildRate = 0;

                // Add in the Military Status Build Rate
                if (research.BeingResearched.apply_military)
                {
                    if (research.BuildingAt.CivilizationInfo.is_military == true)
                        buildRate += research.BuildingAt.InfrastructureInfo.Infrastructure.science_bonus;

                    buildRate += militaryResearchBuildRate;
                }
                else
                {
                    if (research.BuildingAt.CivilizationInfo.is_military == false)
                        buildRate += research.BuildingAt.InfrastructureInfo.Infrastructure.science_bonus;

                    buildRate += civilianResearchBuildRate;
                }
                
                // Append the temp variable to the build percentage
                research.Info.build_percentage += buildRate;

                // And if the item is more than 100% built, we go ahead and build the item, disposing of the R&D in the process
                if (research.Info.build_percentage >= 100)
                {
                    DB_civilization_research civResearch = new DB_civilization_research();
                    civResearch.civilization_id = research.Info.civilization_id;
                    civResearch.game_id = research.Info.game_id;
                    civResearch.research_id = research.Info.research_id;
                    Database.Session.Save(civResearch);

                    Database.Session.Delete(research.Info);
                }
                else // Otherwise, just update the status
                {
                    Database.Session.Update(research.Info);
                }
            }

            int militaryColonialDevelopmentBonus = CalculateColonialDevelopmentBonus(true);
            int civilianColonialDevelopmentBonus = CalculateColonialDevelopmentBonus(false);
            foreach (var infrastructure in IncompleteInfrastructure)
            {
                if (infrastructure.BuildingAt.CivilizationInfo.current_health <= 0) continue;
                int buildRate = 0;

                // Check if the facility we are building it at is equal to the units military status
                buildRate += infrastructure.BuildingAt.Planet.TierInfo.build_rate;
                if (infrastructure.BuildingAt.CivilizationInfo.is_military == infrastructure.BeingBuilt.Infrastructure.is_military)
                    buildRate += infrastructure.BuildingAt.InfrastructureInfo.Infrastructure.colonial_development_bonus;

                // Add in the Military Status Build Rate
                if (infrastructure.BeingBuilt.Infrastructure.is_military)
                    buildRate += militaryColonialDevelopmentBonus;
                else
                    buildRate += civilianColonialDevelopmentBonus;

                // Append the temp variable to the build percentage
                infrastructure.Info.build_percentage += buildRate;
                
                // And if the item is more than 100% built, we go ahead and build the item, disposing of the R&D in the process
                if (infrastructure.Info.build_percentage >= 100)
                {
                    int maxHealth = infrastructure.BeingBuilt.Infrastructure.base_health;
                    foreach (var research in CompletedResearch)
                        if (research.ResearchInfo.apply_infrastructure)
                            maxHealth += research.ResearchInfo.health_bonus;

                    DB_civilization_infrastructure civInfrastructure = new DB_civilization_infrastructure();
                    civInfrastructure.game_id = infrastructure.Info.game_id;
                    civInfrastructure.civilization_id = infrastructure.Info.civilization_id;
                    civInfrastructure.planet_id = infrastructure.Info.planet_id;
                    civInfrastructure.struct_id = infrastructure.Info.struct_id;
                    civInfrastructure.name = infrastructure.Info.name;
                    civInfrastructure.current_health = maxHealth;
                    civInfrastructure.can_upgrade = infrastructure.BeingBuilt.UpgradeInfrastructure.Count > 0;
                    civInfrastructure.is_military = infrastructure.BeingBuilt.Infrastructure.is_military;
                    Database.Session.Save(civInfrastructure);

                    Database.Session.Delete(infrastructure.Info);
                }
                else // Otherwise, just update the status
                {
                    Database.Session.Update(infrastructure.Info);
                }
            }

            int militaryShipConstructionBonus = CalculateShipConstructionBonus(true);
            int civilianShipConstructionBonus = CalculateShipConstructionBonus(false);
            foreach (var ship in IncompleteSpaceUnits)
            {
                if (ship.BuildingAt.CivilizationInfo.current_health <= 0) continue;
                int buildRate = 0;

                // Check if the facility we are building it at is equal to the units military status
                if (ship.BuildingAt.CivilizationInfo.is_military == ship.BeingBuilt.UnitCategory.is_military)
                {
                    buildRate += ship.BeingBuilt.UnitCategory.build_rate;
                    buildRate += ship.BuildingAt.InfrastructureInfo.Infrastructure.ship_construction_bonus;
                }

                // Add in the Military Status Build Rate
                if (ship.BeingBuilt.UnitCategory.is_military)
                    buildRate += militaryShipConstructionBonus;
                else
                    buildRate += civilianShipConstructionBonus;

                // Append the temp variable to the build percentage
                ship.Info.build_percentage += buildRate;

                // And if the item is more than 100% built, we go ahead and build the item, disposing of the R&D in the process
                if (ship.Info.build_percentage >= 100)
                {
                    int maxHealth = ship.BeingBuilt.Info.base_health;
                    foreach (var research in CompletedResearch)
                        if (research.ResearchInfo.apply_ships)
                            maxHealth += research.ResearchInfo.health_bonus;

                    for (int i = 0; i < ship.BeingBuilt.Info.number_to_build; i++)
                    {
                        var newShip = new DB_civilization_units();
                        newShip.battlegroup_id = null;
                        newShip.civilization_id = ship.Info.civilization_id;
                        newShip.current_health = maxHealth;
                        newShip.experience = 0;
                        newShip.game_id = ship.Info.game_id;
                        newShip.gmnotes = "";
                        newShip.name = ship.Info.name;
                        newShip.species_id = ship.Info.species_id;
                        newShip.unit_id = ship.Info.unit_id;
                        Database.Session.Save(newShip);
                    }

                    Database.Session.Delete(ship.Info);
                }
                else // Otherwise, just update the status
                {
                    Database.Session.Update(ship.Info);
                }
            }

            int militaryUnitTrainingBonus = CalculateUnitTrainingBonus(true);
            int civilianUnitTrainingBonus = CalculateUnitTrainingBonus(false);
            foreach (var unit in IncompleteGroundUnits)
            {
                if (unit.BuildingAt.CivilizationInfo.current_health <= 0) continue;
                int buildRate = 0;

                // Check if the facility we are building it at is equal to the units military status
                if (unit.BuildingAt.CivilizationInfo.is_military == unit.BeingBuilt.UnitCategory.is_military)
                {
                    buildRate += unit.BeingBuilt.UnitCategory.build_rate;
                    buildRate += unit.BuildingAt.InfrastructureInfo.Infrastructure.unit_training_bonus;
                }

                // Add in the Military Status Build Rate
                if (unit.BeingBuilt.UnitCategory.is_military)
                    buildRate += militaryUnitTrainingBonus;
                else
                    buildRate += civilianUnitTrainingBonus;

                // Append the temp variable to the build percentage
                unit.Info.build_percentage += buildRate;

                // And if the item is more than 100% built, we go ahead and build the item, disposing of the R&D in the process
                if (unit.Info.build_percentage >= 100)
                {
                    int maxHealth = unit.BeingBuilt.Info.base_health;
                    foreach (var research in CompletedResearch)
                        if (research.ResearchInfo.apply_ships)
                            maxHealth += research.ResearchInfo.health_bonus;

                    var newUnit = new DB_civilization_units();
                    newUnit.battlegroup_id = null;
                    newUnit.civilization_id = unit.Info.civilization_id;
                    newUnit.current_health = maxHealth;
                    newUnit.experience = 0;
                    newUnit.game_id = unit.Info.game_id;
                    newUnit.gmnotes = "";
                    newUnit.name = unit.Info.name;
                    newUnit.species_id = unit.Info.species_id;
                    newUnit.unit_id = unit.Info.unit_id;
                    Database.Session.Save(newUnit);

                    Database.Session.Delete(unit.Info);
                }
                else // Otherwise, just update the status
                {
                    Database.Session.Update(unit.Info);
                }
            }
        }

        public void ProcessHealEverythingToMax()
        {
            foreach (var infrastructure in CompletedInfrastructure)
            {
                infrastructure.CivilizationInfo.current_health = infrastructure.CalculateMaxHealth();
                Database.Session.Update(infrastructure.CivilizationInfo);
            }

            foreach (var unit in CompletedUnitsRaw)
            {
                unit.CivilizationInfo.current_health = unit.CalculateMaxHealth();
                Database.Session.Update(unit.CivilizationInfo);
            }
        }
        #endregion

        public void SortUnitsBattlegroups()
        {
            CompletedSpaceUnits = new List<CivilizationUnit>();
            CompletedGroundUnits = new List<CivilizationUnit>();
            foreach (var unit in CompletedUnitsRaw)
            {
                if (UnitTypes.IsSpaceship(unit.Unit.Info.unit_type))
                    CompletedSpaceUnits.Add(unit);
                else
                    CompletedGroundUnits.Add(unit);
            }

            Battlegroups = new List<Battlegroup>();
            var civilizationBattlegroups = Owner.ThisGame.GameStatistics.BattlegroupsRaw
                .Where(x => x.civilization_id == CivilizationID)
                .ToList();
            foreach (var bg in civilizationBattlegroups)
                Battlegroups.Add(new Battlegroup(bg, Owner));

            foreach (var unit in CompletedUnitsRaw)
            {
                if (unit.CivilizationInfo.battlegroup_id == null) continue;

                var battlegroup = Battlegroups.Where(x => x.ID == unit.CivilizationInfo.battlegroup_id).FirstOrDefault();
                if (battlegroup == null)
                    continue;
                battlegroup.Units.Add(unit);
            }
        }
    }
}
