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

        public List<DB_research> Research;

        public List<Ship> Ships;
        public List<DB_ships> ShipsRaw;
        public List<DB_ship_rates> ShipRatesRaw;

        public List<InfrastructureUpgrade> Infrastructure;
        public List<DB_infrastructure> InfrastructureRaw;
        public List<DB_infrastructure_upgrades> InfrastructureUpgradesRaw;

        public List<DB_planet_tiers> PlanetTiers;

        public GameStatistics(int gameID)
        {
            GameID = gameID;

            QueryResearch();
            QueryShips();
            QueryShipRates();
            QueryInfrastructure();
            QueryInfrastructureUpgrades();
            QueryPlanetTiers();

            ConnectAllValues();
        }

        #region Gets
        public void QueryResearch()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Research", GameID));
            var research = Database.Session.Query<DB_research>().ToList();
            Research = new List<DB_research>(research);
        }

        public void QueryShips()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Ships", GameID));
            var ships = Database.Session.Query<DB_ships>().ToList();
            ShipsRaw = new List<DB_ships>(ships);
        }

        public void QueryShipRates()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Ship Rates", GameID));
            var shipRates = Database.Session.Query<DB_ship_rates>().ToList();
            ShipRatesRaw = new List<DB_ship_rates>(shipRates);
        }

        public void QueryInfrastructure()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure", GameID));
            var infrastructure = Database.Session.Query<DB_infrastructure>().ToList();
            InfrastructureRaw = new List<DB_infrastructure>(infrastructure);
        }

        public void QueryInfrastructureUpgrades()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure Upgrades", GameID));
            var infrastructureUpgrades = Database.Session.Query<DB_infrastructure_upgrades>().ToList();
            InfrastructureUpgradesRaw = new List<DB_infrastructure_upgrades>(infrastructureUpgrades);
        }

        public void QueryPlanetTiers()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Planet Tier", GameID));
            var planetTiers = Database.Session.Query<DB_planet_tiers>().ToList();
            PlanetTiers = new List<DB_planet_tiers>(planetTiers);
        }
        #endregion

        public void ConnectAllValues()
        {
            Ships = new List<Ship>();
            foreach (var ship in ShipsRaw)
            {
                Ship newShip = new Ship();
                newShip.Info = ship;

                if (ship.ship_rate_id == null)
                {
                    newShip.ShipRate = new DB_ship_rates
                    {
                        id = 0,
                        name = "Uncategorized",
                        build_rate = 10
                    };
                    continue;
                }

                newShip.ShipRate = ShipRatesRaw.Where(x => x.id == ship.ship_rate_id).First();
                Ships.Add(newShip);
            }

            Infrastructure = new List<InfrastructureUpgrade>();
            foreach (var infrastructure in InfrastructureRaw)
            {
                InfrastructureUpgrade upgrade = new InfrastructureUpgrade();
                upgrade.Infrastructure = infrastructure;

                var upgradesRaw = InfrastructureUpgradesRaw.Where(x => x.from_infra_id == infrastructure.id).ToList();
                upgrade.Upgrades = new List<DB_infrastructure_upgrades>(upgradesRaw);
            }
        }
    }
}