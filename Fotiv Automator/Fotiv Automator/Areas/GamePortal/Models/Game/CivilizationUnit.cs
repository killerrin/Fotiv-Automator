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
    public class CivilizationUnit
    {
        public int UnitID { get { return Unit.Info.id; } }

        public DB_civilization_units CivilizationInfo;
        public Civilization Owner;

        public Unit Unit;
        public DB_civilization_battlegroups BattlegroupInfo;
        public DB_species SpeciesInfo;

        public CivilizationUnit(DB_civilization_units dbCivilizationUnit, Civilization owner)
        {
            CivilizationInfo = dbCivilizationUnit;
            Owner = owner;
            Unit = new Unit();

            QueryBattlegroup();
        }

        public bool IsInBattlegroup(int battlegroupID)
        {
            if (CivilizationInfo.battlegroup_id == battlegroupID) return true;
            return false;
        }

        public bool IsAlive()
        {
            return CivilizationInfo.current_health > 0;
        }

        public int CalculateMaxHealth()
        {
            int value = Unit.Info.base_health;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.health_bonus;
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = Unit.Info.base_regeneration;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.regeneration_bonus;
            return value;
        }

        public int CalculateAttack()
        {
            int value = Unit.Info.base_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.attack_bonus;
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = Unit.Info.base_special_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.special_attack_bonus;
            return value;
        }

        public int CalculateAgility()
        {
            int value = Unit.Info.base_agility;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_ships)
                    value += research.ResearchInfo.agility_bonus;
            return value;
        }

        #region Queries
        public void QueryBattlegroup()
        {
            if (CivilizationInfo.battlegroup_id == null)
                return;

            Debug.WriteLine(string.Format("Civilization Ship: {0}, Getting Battlegroups", CivilizationInfo.id));
            BattlegroupInfo = Database.Session.Query<DB_civilization_battlegroups>().Where(x => x.id == CivilizationInfo.battlegroup_id).First();
        }
        #endregion
    }
}
