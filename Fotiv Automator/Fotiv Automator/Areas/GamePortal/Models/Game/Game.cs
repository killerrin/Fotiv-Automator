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
    public class Game
    {
        public DB_games Info;
        public GameStatistics GameStatistics { get; protected set; }

        public List<GamePlayer> Players { get; protected set; } 
        public List<Sector> Sectors { get; protected set; }
        public List<Civilization> Civilizations { get; protected set; }

        private Game() { }
        public Game(DB_games game)
        {
            Info = game;

            QueryGameStatistics();

            QueryAllPlayers();
            QueryAllSectors();
            QueryAllCivilizations();

            ConnectAllValues();
        }

        public bool PlayerInGame(int id)
        {
            foreach (var player in Players)
                if (player.User.ID == id)
                    return true;
            return false;
        }

        public bool IsPlayerGM(int id)
        {
            foreach (var player in Players)
                if (player.User.ID == id && player.GameUserInfo.is_gm)
                    return true;
            return false;
        }

        public GamePlayer GetPlayer(int id)
        {
            foreach (var player in Players)
                if (player.User.ID == id)
                    return player;
            return null;
        }

        #region Queries
        public void QueryGameStatistics() { GameStatistics = new GameStatistics(Info.id); }

        public void QueryAllPlayers()
        {
            Players = new List<GamePlayer>();

            Debug.WriteLine(string.Format("Game: {0}, Getting Players", Info.id));
            var dbUsers = Database.Session.Query<DB_users>().ToList();
            var dbGamePlayers = Database.Session.Query<DB_game_users>()
                .Where(x => x.game_id == Info.id)
                .ToList();

            foreach (var dbGamePlayer in dbGamePlayers)
                foreach (var dbUser in dbUsers)
                    if (dbGamePlayer.user_id == dbUser.id)
                        Players.Add(new GamePlayer(dbUser, dbGamePlayer));
        }

        public void QueryAllSectors()
        {
            Sectors = new List<Sector>();

            Debug.WriteLine(string.Format("Game: {0}, Getting Sectors", Info.id));
            var dbSectors = Database.Session.Query<DB_sectors>().ToList();
            var dbGameSectors = Database.Session.Query<DB_game_sectors>()
                .Where(x => x.game_id == Info.id)
                .ToList();

            foreach (var dbGameSector in dbGameSectors)
                foreach (var dbSector in dbSectors)
                    if (dbGameSector.sector_id == dbSector.id)
                        Sectors.Add(new Sector(dbSector, dbGameSector));
        }

        public void QueryAllCivilizations()
        {
            Civilizations = new List<Civilization>();

            Debug.WriteLine(string.Format("Game: {0}, Getting Civilizations", Info.id));
            var dbCivilizations = Database.Session.Query<DB_civilization>().ToList();
            var dbGameCivilizations = Database.Session.Query<DB_game_civilizations>()
                .Where(x => x.game_id == Info.id)
                .ToList();

            foreach (var dbCivilization in dbCivilizations)
                foreach (var dbGameCivilization in dbGameCivilizations)
                    if (dbGameCivilization.civilization_id == dbCivilization.id)
                        Civilizations.Add(new Civilization(dbCivilization));
        }
        #endregion

        public void ConnectAllValues()
        {
            #region Sector Connecting
            foreach (var sector in Sectors)
            {
                foreach (var solarsystem in sector.StarSystemsRaw)
                {
                    foreach (var star in solarsystem.Stars)
                    {
                        #region Satellites and Planet Tier
                        var satellites = star.Planets.Where(x => x.Info.orbiting_planet_id != null);
                        foreach (var planet in star.Planets)
                        {
                            foreach (var satellite in satellites)
                            {
                                if (planet.Info.id == satellite.Info.orbiting_planet_id)
                                    planet.Satellites.Add(satellite);
                            }

                            if (planet.Info.planet_tier_id == null) continue;
                            planet.TierInfo = GameStatistics.PlanetTiers.First(x => x.id == planet.Info.planet_tier_id);
                        }
                        #endregion
                    }

                    #region Jump Gates
                    foreach (var jumpGate in solarsystem.Jumpgates)
                    {
                        bool foundInfrastructure = false;
                        foreach (var civilization in Civilizations)
                        {
                            foreach (var infrastructure in civilization.Infrastructure)
                            {
                                if (infrastructure.Info.id == jumpGate.Info.civ_struct_id)
                                {
                                    foundInfrastructure = true;
                                    break;
                                }
                            }

                            if (foundInfrastructure)
                                break;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Civilization Connections
            foreach (var civilization in Civilizations)
            {
                #region Research
                foreach (var research in civilization.Research)
                {
                    research.ResearchInfo = GameStatistics.Research.Where(x => x.id == research.CivilizationInfo.research_id).First();
                }
                #endregion

                #region Ships
                foreach (var ship in civilization.Ships)
                {
                    ship.Ship = GameStatistics.Ships.Where(x => x.Info.id == ship.Info.ship_id).First();
                }
                #endregion

                #region Infrastructure
                foreach (var infrastructure in civilization.Infrastructure)
                {
                    #region Planet
                    bool foundPlanet = false;
                    foreach (var sector in Sectors)
                    {
                        foreach (var solarsystem in sector.StarSystemsRaw)
                        {
                            foreach (var star in solarsystem.Stars)
                            {
                                foreach (var planet in star.Planets)
                                {
                                    if (infrastructure.Info.planet_id == planet.Info.id)
                                    {
                                        infrastructure.Planet = planet;
                                        foundPlanet = true;
                                        break;
                                    }
                                }

                                if (foundPlanet) break;
                            }

                            if (foundPlanet) break;
                        }

                        if (foundPlanet) break;
                    }
                    #endregion

                    infrastructure.InfrastructureInfo = GameStatistics.Infrastructure
                        .Where(x => x.Infrastructure.id == infrastructure.Info.struct_id)
                        .First();
                }
                #endregion
            }
            #endregion
        }
    }
}
