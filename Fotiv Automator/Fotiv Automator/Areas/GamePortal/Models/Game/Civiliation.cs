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
        public DB_civilization Info;

        public List<CivilizationOwner> Owners;
        public List<Research> Research;
        public List<Infrastructure> Infrastructure;
        public List<CivilizationShip> Ships;

        public List<DB_visited_starsystems> VisitedStarsystemInfo;
        public List<DB_species> SpeciesInfo;
        public List<DB_characters> CharacterInfo;


        public Civilization(DB_civilization dbCivilization)
        {
            Info = dbCivilization;

            QueryOwners();
            QueryResearch();
            QueryInfrastructure();
            QueryShips();

            QueryVisitedStarsystemInfo();
            QuerySpeciesInfo();
            QueryCharacterInfo();
        }

        public bool PlayerOwnsCivilization(int id)
        {
            foreach (var player in Owners)
                if (player.User.ID == id)
                    return true;
            return false;
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
            Research = new List<Models.Game.Research>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Research", Info.id));
            var dbCivilizationResearch = Database.Session.Query<DB_civilization_research>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivResearch in dbCivilizationResearch)
                Research.Add(new Models.Game.Research(dbCivResearch));
        }
        public void QueryInfrastructure()
        {
            Infrastructure = new List<Models.Game.Infrastructure>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Infrastructure", Info.id));
            var dbCivilizationInfrastructure = Database.Session.Query<DB_civilization_infrastructure>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivInfrastructure in dbCivilizationInfrastructure)
                Infrastructure.Add(new Models.Game.Infrastructure(dbCivInfrastructure));
        }
        public void QueryShips()
        {
            Ships = new List<CivilizationShip>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Ships", Info.id));
            var dbCivilizationShips = Database.Session.Query<DB_civilization_ships>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivilizationShip in dbCivilizationShips)
                Ships.Add(new CivilizationShip(dbCivilizationShip));
        }
        #endregion  

        #region Query Raw Info
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

        public void QueryCharacterInfo()
        {
            CharacterInfo = new List<DB_characters>();

            Debug.WriteLine(string.Format("Civilization: {0}, Getting Characters", Info.id));
            var dbCharacters = Database.Session.Query<DB_characters>().ToList();
            var dbCivilizationCharacters = Database.Session.Query<DB_civilization_characters>()
                .Where(x => x.civilization_id == Info.id)
                .ToList();

            foreach (var dbCivilizationCharacter in dbCivilizationCharacters)
                foreach (var dbCharacter in dbCharacters)
                    if (dbCivilizationCharacter.character_id == dbCharacter.id)
                        CharacterInfo.Add(dbCharacter);
        }
        #endregion
        #endregion
    }
}