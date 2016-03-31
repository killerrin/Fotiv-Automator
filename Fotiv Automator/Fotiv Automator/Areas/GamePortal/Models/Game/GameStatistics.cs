﻿using Fotiv_Automator.Models.DatabaseMaps;
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

        public List<Ship> Ships = new List<Ship>();
        public List<DB_ships> ShipsRaw = new List<DB_ships>();
        public List<DB_ship_rates> ShipRatesRaw = new List<DB_ship_rates>();

        public List<InfrastructureUpgrade> Infrastructure = new List<InfrastructureUpgrade>();
        public List<DB_infrastructure> InfrastructureRaw = new List<DB_infrastructure>();
        public List<DB_infrastructure_upgrades> InfrastructureUpgradesRaw = new List<DB_infrastructure_upgrades>();

        public List<DB_planet_tiers> PlanetTiers = new List<DB_planet_tiers>();

        public List<DB_civilization_traits> CivilizationTraits = new List<DB_civilization_traits>();
        public List<DB_species> Species = new List<DB_species>();

        public GameStatistics(int gameID)
        {
            GameID = gameID;

            QueryResearch();
            QueryShips();
            QueryShipRates();
            QueryInfrastructure();
            QueryInfrastructureUpgrades();
            QueryPlanetTiers();
            QueryCivilizationTraits();
            QuerySpecies();

            ConnectAllValues();
        }

        #region Queries
        public void QueryResearch()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Research", GameID));
            var research = Database.Session.Query<DB_research>().ToList();

            Research = new List<DB_research>();
            Research.AddRange(research.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryShips()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Ships", GameID));
            var ships = Database.Session.Query<DB_ships>().ToList();
            ShipsRaw = new List<DB_ships>();
            ShipsRaw.AddRange(ships.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryShipRates()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Ship Rates", GameID));
            var shipRates = Database.Session.Query<DB_ship_rates>().ToList();
            ShipRatesRaw = new List<DB_ship_rates>();
            ShipRatesRaw.AddRange(shipRates.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryInfrastructure()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure", GameID));
            var infrastructure = Database.Session.Query<DB_infrastructure>().ToList();
            InfrastructureRaw = new List<DB_infrastructure>();
            InfrastructureRaw.AddRange(infrastructure.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryInfrastructureUpgrades()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure Upgrades", GameID));
            var infrastructureUpgrades = Database.Session.Query<DB_infrastructure_upgrades>().ToList();
            InfrastructureUpgradesRaw = new List<DB_infrastructure_upgrades>();
            InfrastructureUpgradesRaw.AddRange(infrastructureUpgrades.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryPlanetTiers()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Planet Tier", GameID));
            var planetTiers = Database.Session.Query<DB_planet_tiers>().ToList();
            PlanetTiers = new List<DB_planet_tiers>();
            PlanetTiers.AddRange(planetTiers.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QueryCivilizationTraits()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Civilization Traits", GameID));
            var civilizationTraits = Database.Session.Query<DB_civilization_traits>().ToList();
            CivilizationTraits = new List<DB_civilization_traits>();
            CivilizationTraits.AddRange(civilizationTraits.Where(x => x.game_id == null || x.game_id == GameID));
        }

        public void QuerySpecies()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Species", GameID));
            var species = Database.Session.Query<DB_species>().ToList();
            Species = new List<DB_species>();
            Species.AddRange(species.Where(x => x.game_id == null || x.game_id == GameID));
        }
        #endregion

        public void ConnectAllValues()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Connecting All Values", GameID));

            Ships = new List<Ship>();
            foreach (var ship in ShipsRaw)
            {
                Ship newShip = new Ship();
                newShip.Info = ship;

                if (ship.ship_rate_id != null)
                    newShip.ShipRate = ShipRatesRaw.Where(x => x.id == ship.ship_rate_id).First();
                else
                {
                    newShip.ShipRate = new DB_ship_rates
                    {
                        id = -1,
                        name = "Uncategorized",
                        build_rate = 10
                    };
                }

                Ships.Add(newShip);
            }

            //Debug.WriteLine($"Infrastructure Count {InfrastructureRaw.Count}");

            Infrastructure = new List<InfrastructureUpgrade>();
            foreach (var infrastructure in InfrastructureRaw)
            {
                //Debug.WriteLine($"{infrastructure.name}");

                InfrastructureUpgrade upgrade = new InfrastructureUpgrade();
                upgrade.Infrastructure = infrastructure;

                var upgradesRaw = InfrastructureUpgradesRaw.Where(x => x.from_infra_id == infrastructure.id).ToList();
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
