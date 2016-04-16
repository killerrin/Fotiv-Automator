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
    public class CivilizationShip
    {
        public int ShipID { get { return Ship.Info.id; } }

        public DB_civilization_ships CivilizationInfo;
        public Civilization Owner;

        public Ship Ship;
        public DB_ship_battlegroups BattlegroupInfo;

        public CivilizationShip(DB_civilization_ships dbCivilizationShip, Civilization owner)
        {
            CivilizationInfo = dbCivilizationShip;
            Owner = owner;
            Ship = new Ship();

            QueryBattlegroup();
        }

        public bool IsInBattlegroup(int battlegroupID)
        {
            if (CivilizationInfo.ship_battlegroup_id == battlegroupID) return true;
            return false;
        }

        public bool IsAlive()
        {
            return CivilizationInfo.current_health > 0;
        }

        public int CalculateMaxHealth()
        {
            int value = Ship.Info.base_health;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.health_bonus;
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = Ship.Info.base_regeneration;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.regeneration_bonus;
            return value;
        }

        public int CalculateAttack()
        {
            int value = Ship.Info.base_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.attack_bonus;
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = Ship.Info.base_special_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.special_attack_bonus;
            return value;
        }

        public int CalculateAgility()
        {
            int value = Ship.Info.base_agility;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.agility_bonus;
            return value;
        }

        #region Queries
        public void QueryBattlegroup()
        {
            if (CivilizationInfo.ship_battlegroup_id == null)
                return;

            Debug.WriteLine(string.Format("Civilization Ship: {0}, Getting Battlegroups", CivilizationInfo.id));
            BattlegroupInfo = Database.Session.Query<DB_ship_battlegroups>().Where(x => x.id == CivilizationInfo.ship_battlegroup_id).First();
        }
        #endregion
    }
}
