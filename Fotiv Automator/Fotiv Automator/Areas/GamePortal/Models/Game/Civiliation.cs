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
    public class Civilization
    {
        public int ID { get { return Info.id; } }
        public Game ThisGame;

        public DB_civilization Info;
        public List<CivilizationOwner> Owners = new List<CivilizationOwner>();

        public CivilizationAssets Assets;

        public DB_civilization_traits CivilizationTrait1;
        public DB_civilization_traits CivilizationTrait2;
        public DB_civilization_traits CivilizationTrait3;
        public DB_tech_levels TechLevel;

        public List<DB_visited_starsystems> VisitedStarsystemInfo = new List<DB_visited_starsystems>();
        public List<CivilizationMet> MetCivilizations = new List<CivilizationMet>();

        public List<DB_species> SpeciesInfo = new List<DB_species>();

        public Civilization(DB_civilization dbCivilization, Game game)
        {
            Info = dbCivilization;
            ThisGame = game;

            Assets = new CivilizationAssets(this);

            QueryOwners();

            QueryResearch();
            QueryInfrastructure();
            QueryShips();
            QueryUnits();

            QueryVisitedStarsystemInfo();
            QueryMetCivilizationsInfo();
            QuerySpeciesInfo();
        }

        public bool PlayerOwnsCivilization(int id)
        {
            foreach (var player in Owners)
                if (player.User.ID == id)
                    return true;
            return false;
        }

        public bool CivilizationHasTrait(int id)
        {
            if (CivilizationTrait1?.id == id)
                return true;
            if (CivilizationTrait2?.id == id)
                return true;
            if (CivilizationTrait3?.id == id)
                return true;
            return false;
        }

        public bool CivilizationHasSpecies(int id)
        {
            foreach (var species in SpeciesInfo)
                if (species.id == id)
                    return true;
            return false;
        }

        public bool HasMetCivilization(int id)
        {
            foreach (var metCivilization in MetCivilizations)
                if (metCivilization.Info.civilization_id1 == id || metCivilization.Info.civilization_id2 == id)
                    return true;
            return false;
        }

        public bool HasVisitedSystem(int systemID)
        {
            foreach (var system in VisitedStarsystemInfo)
                if (system.starsystem_id == systemID)
                    return true;
            return false;
        }

        public void ProcessTurn()
        {
            // First we will Increment our Income
            Info.rp += Assets.CalculateIncomePerTurn();
            Database.Session.Update(Info);

            // Next all of our Research and Development
            int militaryResearchBuildRate = Assets.CalculateScienceBuildRate(true);
            int civilianResearchBuildRate = Assets.CalculateScienceBuildRate(false);
            foreach (var research in Assets.IncompleteResearch)
            {
                if (research.ResearchInfo.apply_military)
                {
                    research.CivilizationInfo.build_percentage += militaryResearchBuildRate;
                }
                else
                {
                    research.CivilizationInfo.build_percentage += civilianResearchBuildRate;
                }

                Database.Session.Update(research.CivilizationInfo);
            }

            int militaryColonialDevelopmentBonus = Assets.CalculateColonialDevelopmentBonus(true);
            int civilianColonialDevelopmentBonus = Assets.CalculateColonialDevelopmentBonus(false);
            foreach (var infrastructure in Assets.IncompleteInfrastructure)
            {
                infrastructure.CivilizationInfo.build_percentage += infrastructure.Planet.TierInfo.build_rate;

                if (infrastructure.InfrastructureInfo.Infrastructure.is_military)
                {
                    infrastructure.CivilizationInfo.build_percentage += militaryColonialDevelopmentBonus;
                }
                else
                {
                    infrastructure.CivilizationInfo.build_percentage += civilianColonialDevelopmentBonus;
                }

                Database.Session.Update(infrastructure.CivilizationInfo);
            }

            int militaryShipConstructionBonus = Assets.CalculateShipConstructionBonus(true);
            int civilianShipConstructionBonus = Assets.CalculateShipConstructionBonus(false);
            foreach (var ship in Assets.IncompleteShips)
            {
                ship.CivilizationInfo.build_percentage += ship.Ship.ShipRate.build_rate;

                if (ship.Ship.Info.is_military)
                {
                    ship.CivilizationInfo.build_percentage += militaryShipConstructionBonus;
                }
                else
                {
                    ship.CivilizationInfo.build_percentage += civilianShipConstructionBonus;
                }

                Database.Session.Update(ship.CivilizationInfo);

                if (ship.CivilizationInfo.build_percentage >= 100)
                {
                    for (int i = 1; i < ship.Ship.Info.num_build; i++)
                    {
                        var newShip = ship.CivilizationInfo.Clone(false);
                        Database.Session.Save(newShip);
                    }
                }
            }

            int militaryUnitTrainingBonus = Assets.CalculateUnitTrainingBonus(true);
            int civilianUnitTrainingBonus = Assets.CalculateUnitTrainingBonus(false);
        }

        #region Queries
        #region Query Specialized Data Models
        public void QueryOwners()
        {
            Owners = new List<CivilizationOwner>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Owners", Info.id));
            var dbUsers = Database.Session.Query<DB_users>().ToList();
            var dbUserCivilizations = Database.Session.Query<DB_user_civilizations>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbUserCivilization in dbUserCivilizations)
                foreach (var dbUser in dbUsers)
                    if (dbUserCivilization.user_id == dbUser.id)
                        Owners.Add(new CivilizationOwner(dbUser, dbUserCivilization));
        }
        public void QueryResearch()
        {
            Assets.ResearchRaw = new List<Models.Game.Research>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Research", Info.id));
            var dbCivilizationResearch = Database.Session.Query<DB_civilization_research>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivResearch in dbCivilizationResearch)
                Assets.ResearchRaw.Add(new Models.Game.Research(dbCivResearch, this));
        }
        public void QueryInfrastructure()
        {
            Assets.InfrastructureRaw = new List<Models.Game.Infrastructure>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Infrastructure", Info.id));
            var dbCivilizationInfrastructure = Database.Session.Query<DB_civilization_infrastructure>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivInfrastructure in dbCivilizationInfrastructure)
                Assets.InfrastructureRaw.Add(new Models.Game.Infrastructure(dbCivInfrastructure, this));
        }
        public void QueryShips()
        {
            Assets.ShipsRaw = new List<CivilizationShip>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Ships", Info.id));
            var dbCivilizationShips = Database.Session.Query<DB_civilization_ships>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivilizationShip in dbCivilizationShips)
                Assets.ShipsRaw.Add(new CivilizationShip(dbCivilizationShip, this));
        }
        public void QueryUnits()
        {
            Assets.UnitsRaw = new List<CivilizationUnit>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Units", Info.id));
            var dbCivilizationUnits = Database.Session.Query<DB_civilization_units>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivilizationUnit in dbCivilizationUnits)
                Assets.UnitsRaw.Add(new CivilizationUnit(dbCivilizationUnit, this));
        }
        #endregion

        #region Query Raw Info
        public void QueryMetCivilizationsInfo()
        {
            var metCivilizationsInfo = new List<DB_civilization_met>();

            Debug.WriteLine($"Civilization: {Info.id}, Getting Met Civilizations");
            var dbMetCivilizations = Database.Session.Query<DB_civilization_met>()
                .Where(x => x.civilization_id1 == Info.id || x.civilization_id2 == Info.id)
                .ToList();

            foreach (var dbMetCivilization in dbMetCivilizations)
            {
                var metCivilization = new CivilizationMet(dbMetCivilization);
                metCivilization.CivilizationOne = this;
                MetCivilizations.Add(metCivilization);
            }
        }

        public void QueryVisitedStarsystemInfo()
        {
            VisitedStarsystemInfo = new List<DB_visited_starsystems>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Visited Starsystems", Info.id));
            var dbVisitedSystems = Database.Session.Query<DB_visited_starsystems>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbVisitedSystem in dbVisitedSystems)
                VisitedStarsystemInfo.Add(dbVisitedSystem);
        }

        public void QuerySpeciesInfo()
        {
            SpeciesInfo = new List<DB_species>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Species", Info.id));
            var dbSpecies = Database.Session.Query<DB_species>().ToList();
            var dbCivilizationSpecies = Database.Session.Query<DB_civilization_species>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivilizationSpecie in dbCivilizationSpecies)
                foreach (var dbSpecie in dbSpecies)
                    if (dbCivilizationSpecie.species_id == dbSpecie.id)
                        SpeciesInfo.Add(dbSpecie);
        }
        #endregion
        #endregion
    }
}
