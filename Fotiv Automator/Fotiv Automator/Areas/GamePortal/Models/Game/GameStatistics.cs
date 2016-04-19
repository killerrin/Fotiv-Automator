using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class GameStatistics
    {
        public readonly int GameID;

        public List<DB_research> Research = new List<DB_research>();

        public List<Unit> Units = new List<Unit>();
        public List<DB_units> UnitsRaw = new List<DB_units>();
        public List<DB_unit_categories> UnitCategoriesRaw = new List<DB_unit_categories>();
        public List<DB_civilization_battlegroups> BattlegroupsRaw = new List<DB_civilization_battlegroups>();

        public List<InfrastructureUpgrade> Infrastructure = new List<InfrastructureUpgrade>();
        public List<DB_infrastructure> InfrastructureRaw = new List<DB_infrastructure>();
        public List<DB_infrastructure_upgrades> InfrastructureUpgradesRaw = new List<DB_infrastructure_upgrades>();

        public List<DB_planet_tiers> PlanetTiers = new List<DB_planet_tiers>();
        public List<DB_planet_types> PlanetTypes = new List<DB_planet_types>();
        public List<DB_stage_of_life> StageOfLife = new List<DB_stage_of_life>();
        public List<DB_radiation_levels> RadiationLevels = new List<DB_radiation_levels>();
        public List<DB_star_types> StarTypes = new List<DB_star_types>();
        public List<DB_star_ages> StarAges = new List<DB_star_ages>();

        public List<DB_civilization_traits> CivilizationTraits = new List<DB_civilization_traits>();
        public List<DB_tech_levels> TechLevels = new List<DB_tech_levels>();
        public List<DB_species> Species = new List<DB_species>();

        public List<DB_experience_levels> ExperienceLevels = new List<DB_experience_levels>();

        public GameStatistics(int gameID)
        {
            GameID = gameID;
            QueryAndConnectAll();
        }

        public void QueryAndConnectAll()
        {
            QueryAndConnectUnits();
            QueryAndConnectInfrastructure();

            QueryResearch();
            QueryBattlegroups();

            QueryPlanetTiers();
            QueryPlanetTypes();
            QueryStageOfLife();
            QueryRadiationlevels();
            QueryStarTypes();
            QueryStarAges();

            QueryCivilizationTraits();
            QueryTechLevels();
            QuerySpecies();

            QueryExperience();
        }

        #region Queries
        public void QueryResearch()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Research");
            var research = Database.Session.Query<DB_research>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            Research = new List<DB_research>();
            Research.AddRange(research);
        }

        public void QueryBattlegroups()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Battlegroups");
            var battlegroup = Database.Session.Query<DB_civilization_battlegroups>()
                .Where(x => x.game_id == GameID)
                .ToList();
            BattlegroupsRaw = new List<DB_civilization_battlegroups>();
            BattlegroupsRaw.AddRange(battlegroup);
        }

        public void QueryPlanetTiers()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Planet Tier");
            var planetTiers = Database.Session.Query<DB_planet_tiers>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            PlanetTiers = new List<DB_planet_tiers>();
            PlanetTiers.AddRange(planetTiers);
        }

        public void QueryPlanetTypes()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Planet Types");
            var planetTypes = Database.Session.Query<DB_planet_types>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            PlanetTypes = new List<DB_planet_types>();
            PlanetTypes.AddRange(planetTypes);
        }

        public void QueryStageOfLife()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Stage of Life");
            var stageOfLife = Database.Session.Query<DB_stage_of_life>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            StageOfLife = new List<DB_stage_of_life>();
            StageOfLife.AddRange(stageOfLife);
        }

        public void QueryRadiationlevels()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Radiation Levels");
            var radiationLevels = Database.Session.Query<DB_radiation_levels>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            RadiationLevels = new List<DB_radiation_levels>();
            RadiationLevels.AddRange(radiationLevels);
        }

        public void QueryStarTypes()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Star Types");
            var starTypes = Database.Session.Query<DB_star_types>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            StarTypes = new List<DB_star_types>();
            StarTypes.AddRange(starTypes);
        }

        public void QueryStarAges()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Star Ages");
            var starAges = Database.Session.Query<DB_star_ages>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            StarAges = new List<DB_star_ages>();
            StarAges.AddRange(starAges);
        }

        public void QueryCivilizationTraits()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Civilization Traits");
            var civilizationTraits = Database.Session.Query<DB_civilization_traits>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            CivilizationTraits = new List<DB_civilization_traits>();
            CivilizationTraits.AddRange(civilizationTraits);
        }

        public void QueryTechLevels()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Tech Levels");
            var techLevels = Database.Session.Query<DB_tech_levels>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            TechLevels = new List<DB_tech_levels>();
            TechLevels.AddRange(techLevels);
        }

        public void QuerySpecies()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Species");
            var species = Database.Session.Query<DB_species>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            Species = new List<DB_species>();
            Species.AddRange(species);
        }

        public void QueryExperience()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Experience");
            var experience = Database.Session.Query<DB_experience_levels>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            ExperienceLevels = new List<DB_experience_levels>();
            ExperienceLevels.AddRange(experience);
        }

        #endregion

        public void QueryAndConnectUnits()
        {
            Debug.WriteLine($"GameStatistics: {GameID}, Getting Units");
            var ships = Database.Session.Query<DB_units>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            UnitsRaw = new List<DB_units>();
            UnitsRaw.AddRange(ships);

            Debug.WriteLine($"GameStatistics: {GameID}, Getting Unit Categories");
            var shipRates = Database.Session.Query<DB_unit_categories>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            UnitCategoriesRaw = new List<DB_unit_categories>();
            UnitCategoriesRaw.AddRange(shipRates);

            Debug.WriteLine($"GameStatistics: GameID={GameID}, Connecting Units");
            Units = new List<Unit>();
            foreach (var unit in UnitsRaw)
            {
                Unit newUnit = new Unit();
                newUnit.Info = unit;

                if (unit.unit_category_id != null)
                    newUnit.UnitCategory = UnitCategoriesRaw.Where(x => x.id == unit.unit_category_id).First();
                else
                {
                    newUnit.UnitCategory = new DB_unit_categories
                    {
                        id = -1,
                        name = "Uncategorized",
                        build_rate = 10,
                        is_military = true
                    };
                }

                Units.Add(newUnit);
            }
        }

        public void QueryAndConnectInfrastructure()
        {
            Debug.WriteLine($"GameStatistics: GameID={GameID}, Getting Infrastructure");
            var infrastructure = Database.Session.Query<DB_infrastructure>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            InfrastructureRaw = new List<DB_infrastructure>();
            InfrastructureRaw.AddRange(infrastructure);

            Debug.WriteLine($"GameStatistics: GameID={GameID}, Getting Infrastructure Upgrades");
            var infrastructureUpgrades = Database.Session.Query<DB_infrastructure_upgrades>()
                .Where(x => x.game_id == null || x.game_id == GameID)
                .ToList();
            InfrastructureUpgradesRaw = new List<DB_infrastructure_upgrades>();
            InfrastructureUpgradesRaw.AddRange(infrastructureUpgrades);

            Debug.WriteLine($"GameStatistics: GameID={GameID}, Connecting Infrastructure");
            Infrastructure = new List<InfrastructureUpgrade>();
            foreach (var rawInfrastructure in InfrastructureRaw)
            {
                InfrastructureUpgrade upgrade = new InfrastructureUpgrade();
                upgrade.Infrastructure = rawInfrastructure;

                var upgradesRaw = InfrastructureUpgradesRaw.Where(x => x.from_infra_id == rawInfrastructure.id).ToList();
                upgrade.Upgrades = new List<DB_infrastructure_upgrades>(upgradesRaw);

                upgrade.UpgradeInfrastructure = new List<DB_infrastructure>();
                foreach (var upgradeRaw in upgrade.Upgrades)
                {
                    var infraRaw = InfrastructureRaw.Where(x => x.id == upgradeRaw.to_infra_id).First();
                    upgrade.UpgradeInfrastructure.Add(infraRaw);
                }

                Infrastructure.Add(upgrade);
            }
        }
    }
}
