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
                if (research.BeingResearched.apply_military)
                {
                    research.Info.build_percentage += militaryResearchBuildRate;
                }
                else
                {
                    research.Info.build_percentage += civilianResearchBuildRate;
                }

                Database.Session.Update(research.Info);
            }

            int militaryColonialDevelopmentBonus = Assets.CalculateColonialDevelopmentBonus(true);
            int civilianColonialDevelopmentBonus = Assets.CalculateColonialDevelopmentBonus(false);
            foreach (var infrastructure in Assets.IncompleteInfrastructure)
            {
                infrastructure.Info.build_percentage += infrastructure.BuildingAt.Planet.TierInfo.build_rate;

                if (infrastructure.BeingBuilt.Infrastructure.is_military)
                {
                    infrastructure.Info.build_percentage += militaryColonialDevelopmentBonus;
                }
                else
                {
                    infrastructure.Info.build_percentage += civilianColonialDevelopmentBonus;
                }

                Database.Session.Update(infrastructure.Info);
            }

            int militaryShipConstructionBonus = Assets.CalculateShipConstructionBonus(true);
            int civilianShipConstructionBonus = Assets.CalculateShipConstructionBonus(false);
            foreach (var ship in Assets.IncompleteSpaceUnits)
            {
                ship.Info.build_percentage += ship.BeingBuilt.UnitCategory.build_rate;

                if (ship.BeingBuilt.UnitCategory.is_military)
                {
                    ship.Info.build_percentage += militaryShipConstructionBonus;
                }
                else
                {
                    ship.Info.build_percentage += civilianShipConstructionBonus;
                }

                if (ship.Info.build_percentage >= 100)
                {
                    int maxHealth = ship.BeingBuilt.Info.base_health;
                    foreach (var research in Assets.CompletedResearch)
                        if (research.ResearchInfo.apply_ships)
                            maxHealth += research.ResearchInfo.health_bonus;

                    for (int i = 0; i < ship.BeingBuilt.Info.number_to_build; i++)
                    {
                        var newShip = new DB_civilization_units();
                        newShip.battlegroup_id = null;
                        newShip.civilization_id = ID;
                        newShip.current_health = maxHealth;
                        newShip.experience = 0;
                        newShip.game_id = ThisGame.ID;
                        newShip.gmnotes = "";
                        newShip.name = ship.Info.name;
                        newShip.species_id = ship.Info.species_id;
                        newShip.unit_id = ship.Info.unit_id;
                        Database.Session.Save(newShip);
                    }

                    Database.Session.Delete(ship.Info);
                }
                else
                {
                    Database.Session.Update(ship.Info);
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
            Debug.WriteLine(string.Format("Civilization: {0}, Getting Research", Info.id));

            // RnD Development
            var dbRNDResearch = Database.Session.Query<DB_civilization_rnd_research>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.IncompleteResearch = new List<RnDResearch>();
            foreach (var db in dbRNDResearch)
                Assets.IncompleteResearch.Add(new RnDResearch(db, this));

            // Completed Research
            var dbCivilizationResearch = Database.Session.Query<DB_civilization_research>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.CompletedResearch = new List<Research>();
            foreach (var dbCivResearch in dbCivilizationResearch)
                Assets.CompletedResearch.Add(new Models.Game.Research(dbCivResearch, this));


        }
        public void QueryInfrastructure()
        {
            Debug.WriteLine(string.Format("Civilization: {0}, Getting Infrastructure", Info.id));

            // RnD Infrastructure
            var dbRNDInfrastructure = Database.Session.Query<DB_civilization_rnd_infrastructure>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.IncompleteInfrastructure = new List<RnDInfrastructure>();
            foreach (var db in dbRNDInfrastructure)
                Assets.IncompleteInfrastructure.Add(new RnDInfrastructure(db, this));

            // Completed Infrastructure
            var dbCivilizationInfrastructure = Database.Session.Query<DB_civilization_infrastructure>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.CompletedInfrastructure = new List<Infrastructure>();
            foreach (var dbCivInfrastructure in dbCivilizationInfrastructure)
                Assets.CompletedInfrastructure.Add(new Models.Game.Infrastructure(dbCivInfrastructure, this));
        }
        public void QueryUnits()
        {
            Debug.WriteLine(string.Format("Civilization: {0}, Getting Units", Info.id));

            // RnD Development
            var dbRNDUnits = Database.Session.Query<DB_civilization_rnd_units>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.IncompleteUnitsRaw = new List<RnDUnit>();
            foreach (var db in dbRNDUnits)
                Assets.IncompleteUnitsRaw.Add(new RnDUnit(db, this));

            // Completed Units
            var dbCivilizationUnits = Database.Session.Query<DB_civilization_units>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            Assets.CompletedUnitsRaw = new List<CivilizationUnit>();
            foreach (var db in dbCivilizationUnits)
                Assets.CompletedUnitsRaw.Add(new CivilizationUnit(db, this));
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
