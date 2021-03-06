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

        public bool IsPlayerInGame(int id)
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

        public void QueryAndConnectCivilizations()
        {
            Debug.WriteLine(string.Format("Game: {0}, Getting Civilizations", Info.id));
            var dbCivilizations = Database.Session.Query<DB_civilization>()
                .Where(x => x.game_id == Info.id)
                .ToList();

            Civilizations = new List<Civilization>();
            foreach (var dbCivilization in dbCivilizations)
                Civilizations.Add(new Civilization(dbCivilization, this));

            Debug.WriteLine($"Game: {Info.id}, Connecting Civilizations");
            foreach (var civilization in Civilizations)
            {
                #region Completed Assets
                // Research
                foreach (var research in civilization.Assets.CompletedResearch)
                {
                    research.ResearchInfo = GameStatistics.Research.Where(x => x.id == research.CivilizationInfo.research_id).First();
                }

                // Units
                foreach (var unit in civilization.Assets.CompletedUnitsRaw)
                {
                    unit.Unit = GameStatistics.Units.Where(x => x.Info.id == unit.CivilizationInfo.unit_id).First();

                    if (unit.CivilizationInfo.battlegroup_id != null)
                        unit.BattlegroupInfo = GameStatistics.BattlegroupsRaw.Where(x => x.id == unit.CivilizationInfo.battlegroup_id).First();

                    if (unit.CivilizationInfo.species_id != null)
                        unit.SpeciesInfo = GameStatistics.Species.Where(x => x.id == unit.CivilizationInfo.species_id).First();

                    var experienceLevels = GameStatistics.ExperienceLevels.Where(x => x.threshold < unit.CivilizationInfo.experience).ToList();
                    var highestThreshold = experienceLevels.OrderByDescending(x => x.threshold).FirstOrDefault();
                    if (highestThreshold == null)
                    {
                        unit.ExperienceLevel = new DB_experience_levels
                        {
                            id = -1,
                            game_id = Info.id,
                            name = "Level 0",
                            threshold = 0,
                            agility_bonus = 0,
                            attack_bonus = 0,
                            health_bonus = 0,
                            regeneration_bonus = 0,
                            special_attack_bonus = 0
                        };
                    }
                    else
                        unit.ExperienceLevel = highestThreshold;
                }

                // Infrastructure
                foreach (var infrastructure in civilization.Assets.CompletedInfrastructure)
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

                    // Experience
                    var experienceLevels = GameStatistics.ExperienceLevels.Where(x => x.threshold < infrastructure.CivilizationInfo.experience).ToList();
                    var highestThreshold = experienceLevels.OrderByDescending(x => x.threshold).FirstOrDefault();
                    if (highestThreshold == null)
                    {
                        infrastructure.ExperienceLevel = new DB_experience_levels
                        {
                            id = -1,
                            game_id = Info.id,
                            name = "Level 0",
                            threshold = 0,
                            agility_bonus = 0,
                            attack_bonus = 0,
                            health_bonus = 0,
                            regeneration_bonus = 0,
                            special_attack_bonus = 0
                        };
                    }
                    else
                        infrastructure.ExperienceLevel = highestThreshold;
                }
                #endregion

                #region Research and Development
                foreach (var research in civilization.Assets.IncompleteResearch)
                {
                    research.BeingResearched = GameStatistics.Research.Where(x => x.id == research.Info.research_id).First();
                    research.BuildingAt = civilization.Assets.CompletedInfrastructure.Where(x => x.CivilizationInfo.id == research.Info.civ_struct_id).First();
                }

                civilization.Assets.IncompleteGroundUnits = new List<RnDUnit>();
                civilization.Assets.IncompleteSpaceUnits = new List<RnDUnit>();
                foreach (var unit in civilization.Assets.IncompleteUnitsRaw)
                {
                    unit.BeingBuilt = GameStatistics.Units.Where(x => x.Info.id == unit.Info.unit_id).First();
                    unit.BuildingAt = civilization.Assets.CompletedInfrastructure.Where(x => x.CivilizationInfo.id == unit.Info.civ_struct_id).First();

                    if (UnitTypes.IsSpaceship(unit.BeingBuilt.Info.unit_type))
                        civilization.Assets.IncompleteSpaceUnits.Add(unit);
                    else
                        civilization.Assets.IncompleteGroundUnits.Add(unit);
                }

                foreach (var infrastructure in civilization.Assets.IncompleteInfrastructure)
                {
                    infrastructure.BeingBuilt = GameStatistics.Infrastructure.Where(x => x.Infrastructure.id == infrastructure.Info.struct_id).First();
                    infrastructure.BuildingAt = civilization.Assets.CompletedInfrastructure.Where(x => x.CivilizationInfo.id == infrastructure.Info.civ_struct_id).First();

                    if (Sector != null)
                        infrastructure.Planet = Sector.PlanetFromID(infrastructure.Info.planet_id);
                }
                #endregion

                civilization.Assets.SortUnitsBattlegroups();

                if (Sector != null)
                    foreach (var battlegroup in civilization.Assets.Battlegroups)
                        battlegroup.StarSystem = Sector.StarsystemFromID(battlegroup.Info.starsystem_id);


                #region Civilization Traits && TechLevel
                if (civilization.Info.civilization_traits_1_id != null)
                    civilization.CivilizationTrait1 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_1_id).First();

                if (civilization.Info.civilization_traits_2_id != null)
                    civilization.CivilizationTrait2 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_2_id).First();

                if (civilization.Info.civilization_traits_3_id != null)
                    civilization.CivilizationTrait3 = GameStatistics.CivilizationTraits.Where(x => x.id == civilization.Info.civilization_traits_3_id).First();

                if (civilization.Info.tech_level_id != null)
                    civilization.TechLevel = GameStatistics.TechLevels.Where(x => x.id == civilization.Info.tech_level_id).First();
                #endregion

                if (civilization.MetCivilizations.Count > 0)
                {
                    var allCivilizationsButThis = Civilizations.Where(x => x.ID != civilization.ID);

                    foreach (var metCivilizations in civilization.MetCivilizations)
                    {
                        foreach (var allCivization in allCivilizationsButThis)
                        {
                            if (allCivization.ID == metCivilizations.Info.civilization_id1 ||
                                allCivization.ID == metCivilizations.Info.civilization_id2)
                            {
                                metCivilizations.CivilizationTwo = allCivization;
                            }
                        }
                    }
                }
            }
        }

        public void QueryAndConnectSector()
        {
            Debug.WriteLine($"Game: {Info.id}, QueryAndConnectSector - BEGIN");
            #region Query Sector
            // Get the Sector
            var dbSectors = Database.Session.Query<DB_sectors>()
                .Where(x => x.game_id == Info.id)
                .ToList();

            if (dbSectors.Count > 0)
            {
                Sector = new Sector(dbSectors[0]);

                #region Get the Stars
                Debug.WriteLine($"Sector: {Sector.Info.id}, Getting Star Systems");

                var dbSystems = Database.Session.Query<DB_starsystems>()
                    .Where(x => x.sector_id == Sector.ID)
                    .ToList();

                var dbStars = Database.Session.Query<DB_stars>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                var dbPlanets = Database.Session.Query<DB_planets>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                var dbJumpgates = Database.Session.Query<DB_jumpgates>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                var dbWormholes = Database.Session.Query<DB_wormholes>()
                    .Where(x => x.game_id == Info.id)
                    .ToList();

                // Add in all the Starsystems
                Sector.StarSystemsRaw = new List<Starsystem>();
                foreach (var dbSystem in dbSystems)
                {
                    Debug.Write($"Sector: {Sector.Info.id}, Getting Star System id={dbSystem.id}");

                    var starsystem = new Starsystem(dbSystem);
                    starsystem.Sector = Sector;
                    Sector.StarSystemsRaw.Add(starsystem);

                    // Create the Stars
                    starsystem.Stars = new List<Star>();
                    foreach (var dbStar in dbStars)
                    {
                        if (dbStar.starsystem_id == starsystem.ID)
                        {
                            Debug.Write($"\t Star id={dbStar.id}");

                            var star = new Star(dbStar);
                            star.SolarSystem = starsystem;
                            starsystem.Stars.Add(star);

                            // Add the planets
                            star.Planets = new List<Planet>();
                            foreach (var dbPlanet in dbPlanets)
                            {
                                if (dbPlanet.star_id == star.ID)
                                {
                                    //Debug.Write($"\t Planet: Getting Planet id={dbPlanet.id}");
                                    var planet = new Planet(dbPlanet);
                                    planet.Star = star;
                                    star.Planets.Add(planet);
                                }
                            }
                        }
                    }

                    starsystem.Jumpgates = new List<Jumpgate>();
                    foreach (var dbJumpgate in dbJumpgates)
                    {
                        if (dbJumpgate.from_system_id == starsystem.ID)
                        {
                            var jumpgate = new Jumpgate(dbJumpgate);
                            jumpgate.FromSystem = starsystem;
                            starsystem.Jumpgates.Add(jumpgate);
                        }
                    }

                    starsystem.Wormholes = new List<Wormhole>();
                    foreach (var dbWormhole in dbWormholes)
                    {
                        if (dbWormhole.system_id_one == starsystem.ID || dbWormhole.system_id_two == starsystem.ID)
                        {
                            var wormhole = new Wormhole(dbWormhole);
                            starsystem.Wormholes.Add(wormhole);
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
                    foreach (var civilization in Civilizations)
                    {
                        foreach (var battlegroup in civilization.Assets.Battlegroups)
                            if (solarsystem.Info.id == battlegroup.Info.starsystem_id)
                                battlegroup.StarSystem = solarsystem;
                    }

                    foreach (var wormhole in solarsystem.Wormholes)
                    {
                        wormhole.SystemOne = Sector.StarsystemFromID(wormhole.Info.system_id_one);
                        wormhole.SystemTwo = Sector.StarsystemFromID(wormhole.Info.system_id_two);
                    }

                    foreach (var jumpGate in solarsystem.Jumpgates)
                    {
                        jumpGate.ToSystem = Sector.StarsystemFromID(jumpGate.Info.to_system_id);

                        foreach (var civilization in Civilizations)
                        {
                            var infrastructure = civilization.Assets.CompletedInfrastructure
                                .Where(x => x.CivilizationInfo.id == jumpGate.Info.civ_struct_id)
                                .FirstOrDefault();

                            if (infrastructure == null) continue;
                            jumpGate.Infrastructure = infrastructure;
                            break;
                        }
                    }

                    foreach (var star in solarsystem.Stars)
                    {
                        #region Satellites and Planet Tier
                        var satellites = star.Planets.Where(x => x.Info.orbiting_planet_id != null);
                        foreach (var planet in star.Planets)
                        {
                            foreach (var civilization in Civilizations)
                            {
                                foreach (var infrastructure in civilization.Assets.CompletedInfrastructure)
                                {
                                    if (infrastructure.CivilizationInfo.planet_id == planet.Info.id)
                                    {
                                        planet.Infrastructure.Add(infrastructure);
                                        infrastructure.Planet = planet;
                                    }
                                }

                                foreach (var infrastructure in civilization.Assets.IncompleteInfrastructure)
                                {
                                    if (infrastructure.Info.planet_id == planet.Info.id)
                                    {
                                        infrastructure.Planet = planet;
                                    }
                                }
                            }

                            foreach (var satellite in satellites)
                            {
                                if (planet.Info.id == satellite.Info.orbiting_planet_id)
                                    planet.Satellites.Add(satellite);
                            }

                            if (planet.Info.planet_tier_id != null)
                                planet.TierInfo = GameStatistics.PlanetTiers.First(x => x.id == planet.Info.planet_tier_id);
                            else
                            {
                                planet.TierInfo = new DB_planet_tiers
                                {
                                    id = -1,
                                    name = "None",
                                    build_rate = 10
                                };
                            }

                            if (planet.Info.planet_type_id != null)
                                planet.TypeInfo = GameStatistics.PlanetTypes.First(x => x.id == planet.Info.planet_type_id);
                            else
                            {
                                planet.TypeInfo = new DB_planet_types
                                {
                                    id = -1,
                                    name = "None",
                                };
                            }
                            if (planet.Info.stage_of_life_id != null)
                                planet.StageOfLifeInfo = GameStatistics.StageOfLife.First(x => x.id == planet.Info.stage_of_life_id);
                            else
                            {
                                planet.StageOfLifeInfo = new DB_stage_of_life
                                {
                                    id = -1,
                                    name = "None"
                                };
                            }
                        }
                        #endregion

                        if (star.Info.star_type_id != null)
                            star.StarTypeInfo = GameStatistics.StarTypes.First(x => x.id == star.Info.star_type_id);
                        else
                        {
                            star.StarTypeInfo = new DB_star_types
                            {
                                id = -1,
                                name = "None"
                            };
                        }

                        if (star.Info.star_age_id != null)
                            star.StarAgeInfo = GameStatistics.StarAges.First(x => x.id == star.Info.star_age_id);
                        else
                        {
                            star.StarAgeInfo = new DB_star_ages
                            {
                                id = -1,
                                name = "None"
                            };
                        }

                        if (star.Info.radiation_level_id != null)
                            star.RadiationLevelInfo = GameStatistics.RadiationLevels.First(x => x.id == star.Info.radiation_level_id);
                        else
                        {
                            star.RadiationLevelInfo = new DB_radiation_levels
                            {
                                id = -1,
                                name = "None"
                            };
                        }
                    }
                }
            }
            #endregion

            Debug.WriteLine($"Game: {Info.id}, QueryAndConnectSector - END");
        }

    }
}
