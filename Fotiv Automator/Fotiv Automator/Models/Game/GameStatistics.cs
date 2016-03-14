using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class GameStatistics
    {
        public readonly int GameID;

        public List<DB_research> Research;

        public List<DB_ships> Ships;
        public List<DB_ship_rates> ShipRates;

        public List<DB_infrastructure> Infrastructure;
        public List<DB_infrastructure_upgrades> InfrastructureUpgrades;

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
            Ships = new List<DB_ships>(ships);
        }

        public void QueryShipRates()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Ship Rates", GameID));
            var shipRates = Database.Session.Query<DB_ship_rates>().ToList();
            ShipRates = new List<DB_ship_rates>(shipRates);
        }

        public void QueryInfrastructure()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure", GameID));
            var infrastructure = Database.Session.Query<DB_infrastructure>().ToList();
            Infrastructure = new List<DB_infrastructure>(infrastructure);
        }

        public void QueryInfrastructureUpgrades()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Infrastructure Upgrades", GameID));
            var infrastructureUpgrades = Database.Session.Query<DB_infrastructure_upgrades>().ToList();
            InfrastructureUpgrades = new List<DB_infrastructure_upgrades>(infrastructureUpgrades);
        }

        public void QueryPlanetTiers()
        {
            Debug.WriteLine(string.Format("GameStatistics: {0}, Getting Planet Tier", GameID));
            var planetTiers = Database.Session.Query<DB_planet_tiers>().ToList();
            PlanetTiers = new List<DB_planet_tiers>(planetTiers);
        }
        #endregion
    }
}
