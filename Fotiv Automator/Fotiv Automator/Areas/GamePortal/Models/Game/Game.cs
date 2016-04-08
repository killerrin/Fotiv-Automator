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
        public int ID { get { return Info.id; } }

        public DB_games Info;
        public GameStatistics GameStatistics { get; protected set; }
        public Sector Sector { get; protected set; }

        public List<GamePlayer> Players { get; protected set; } = new List<GamePlayer>();
        public List<Civilization> Civilizations { get; protected set; } = new List<Civilization>();

        public Random Random = new Random();

        private Game() { }
        public Game(DB_games game)
        {
            Info = game;

            QueryAllPlayers();
            QueryGameStatistics();

            QueryAndConnectCivilizations();
            QueryAndConnectSector();
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

        public Civilization GetCivilization(int id)
        {
            foreach (var civilization in Civilizations)
                if (civilization.ID == id)
                    return civilization;
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
        #endregion

        public void QueryAndConnectSector()
        {
            Debug.WriteLine($"Game: {Info.id}, QueryAndConnectSector - BEGIN");
            #region Query Sector
            // Get the Sector
            var dbSectors = Database.Session.Query<DB_sectors>()
                .Where(x => x.game_id == Info.id || x.game_id == null)
                .ToList();

            if (dbSectors.Count > 0)
            {
                Sector = new Sector(dbSectors[0]);

                #region Get the Stars
                Debug.WriteLine($"Sector: {Sector.Info.id}, Getting Star Systems");

                var dbSystems = Database.Session.Query<DB_starsystems>()
                    .Where(x => x.sector_id == Info.id)
                    .ToList();

                var dbStars = Database.Session.Query<DB_stars>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                var dbPlanets = Database.Session.Query<DB_planets>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                // Add in all the Starsystems
                Sector.StarSystemsRaw = new List<Starsystem>();
                foreach (var dbSystem in dbSystems)
                {
                    var starsystem = new Starsystem(dbSystem);
                    Sector.StarSystemsRaw.Add(starsystem);

                    // Create the Stars
                    starsystem.Stars = new List<Star>();
                    foreach (var dbStar in dbStars)
                    {
                        if (dbStar.starsystem_id == starsystem.ID)
                        {
                            var star = new Star(dbStar);
                            starsystem.Stars.Add(star);

                            // And finally the planets
                            star.Planets = new List<Planet>();
                            foreach (var planet in dbPlanets)
                                if (planet.star_id == star.ID)
                                    star.Planets.Add(new Planet(planet));
                        }
                    }

                }


                // Create the 2D List of StarSystems
                Sector.StarSystems = new List<List<Starsystem>>();
                var sortingList = new List<Starsystem>(Sector.StarSystemsRaw);
                while (sortingList.Count > 0)
                {
                    var newColumn = new List<Starsystem>();
                    int currentX = Sector.StarSystems.Count;
                    int currentY = 0;

                    while (true)
                    {
                        int columnCount = newColumn.Count;
                        for (int i = sortingList.Count - 1; i >= 0; i--)
                        {
                            if (sortingList[i].HexCode.IsCoordinate(currentX, currentY))
                            {
                                newColumn.Add(sortingList[i]);
                                currentY++;

                                sortingList.RemoveAt(i);
                                continue;
                            }
                        }

                        if (columnCount == newColumn.Count)
                            break;
                    }

                    if (newColumn.Count > 0) Sector.StarSystems.Add(newColumn);
                }
                #endregion
            }
            #endregion

            #region Sector Connecting
            if (Sector != null)
            {
                foreach (var solarsystem in Sector.StarSystemsRaw)
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

                    ConnectJumpGates(solarsystem.Jumpgates);
                }
            }
            #endregion

            Debug.WriteLine($"Game: {Info.id}, QueryAndConnectSector - END");
        }

        public void QueryAndConnectCivilizations()
        {
            Debug.WriteLine(string.Format("Game: {0}, Getting Civilizations", Info.id));
            var dbCivilizations = Database.Session.Query<DB_civilization>()
                .Where(x => x.game_id == Info.id || x.game_id == null)
                .ToList();

            Civilizations = new List<Civilization>();
            foreach (var dbCivilization in dbCivilizations)
                Civilizations.Add(new Civilization(dbCivilization));

            Debug.WriteLine($"Game: {Info.id}, Connecting Civilizations");
            foreach (var civilization in Civilizations)
            {
                #region Research
                foreach (var research in civilization.Assets.ResearchRaw)
                {
                    research.ResearchInfo = GameStatistics.Research.Where(x => x.id == research.CivilizationInfo.research_id).First();
                }
                #endregion

                #region Ships
                foreach (var ship in civilization.Assets.ShipsRaw)
                {
                    ship.Ship = GameStatistics.Ships.Where(x => x.Info.id == ship.CivilizationInfo.ship_id).First();
                }
                #endregion

                #region Infrastructure
                foreach (var infrastructure in civilization.Assets.InfrastructureRaw)
                {
                    #region Planet
                    bool foundPlanet = false;
                    if (Sector != null)
                    {
                        foreach (var solarsystem in Sector.StarSystemsRaw)
                        {
                            foreach (var star in solarsystem.Stars)
                            {
                                foreach (var planet in star.Planets)
                                {
                                    if (infrastructure.CivilizationInfo.planet_id == planet.Info.id)
                                    {
                                        infrastructure.Planet = planet;
                                        foundPlanet = true;

                                        planet.Infrastructure.Add(infrastructure);
                                        break;
                                    }
                                }

                                if (foundPlanet) break;
                            }

                            if (foundPlanet) break;
                        }
                    }
                    #endregion

                    infrastructure.InfrastructureInfo = GameStatistics.Infrastructure
                        .Where(x => x.Infrastructure.id == infrastructure.CivilizationInfo.struct_id)
                        .First();


                }
                #endregion

                civilization.Assets.SortCompletedIncomplete();

                #region Civilization Traits
                if (civilization.Info.civilization_traits_1_id != null)
                    civilization.CivilizationTrait1 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_1_id).First();

                if (civilization.Info.civilization_traits_2_id != null)
                    civilization.CivilizationTrait2 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_2_id).First();

                if (civilization.Info.civilization_traits_3_id != null)
                    civilization.CivilizationTrait3 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_3_id).First();
                #endregion
            }
        }

        private void ConnectJumpGates(List<Jumpgate> jumpgates)
        {
            //Debug.WriteLine($"Game: {Info.id}, Connecting Jumpgates");

            foreach (var jumpGate in jumpgates)
            {
                foreach (var civilization in Civilizations)
                {
                    var infrastructure = civilization.Assets.InfrastructureRaw
                        .Where(x => x.CivilizationInfo.id == jumpGate.Info.civ_struct_id)
                        .FirstOrDefault();

                    if (infrastructure == null) continue;
                    jumpGate.Infrastructure = infrastructure;
                    break;
                }
            }
        }
    }
}
