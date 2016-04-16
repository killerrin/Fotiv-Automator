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
        public int UnitID { get { return Unit.id; } }

        public DB_civilization_units CivilizationInfo;
        public Civilization Owner;

        public DB_units Unit;
        public DB_unit_battlegroups BattlegroupInfo;

        public CivilizationUnit(DB_civilization_units dbCivilizationUnit, Civilization owner)
        {
            CivilizationInfo = dbCivilizationUnit;
            Owner = owner;
            Unit = new DB_units();

            QueryBattlegroup();
        }

        public bool IsInBattlegroup(int battlegroupID)
        {
            if (CivilizationInfo.unit_battlegroup_id == battlegroupID) return true;
            return false;
        }

        public bool IsAlive()
        {
            return CivilizationInfo.current_health > 0;
        }

        public int CalculateMaxHealth()
        {
            int value = Unit.base_health;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_units)
                    value += research.ResearchInfo.health_bonus;
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = Unit.base_regeneration;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_units)
                    value += research.ResearchInfo.regeneration_bonus;
            return value;
        }

        public int CalculateAttack()
        {
            int value = Unit.base_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_units)
                    value += research.ResearchInfo.attack_bonus;
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = Unit.base_special_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_units)
                    value += research.ResearchInfo.special_attack_bonus;
            return value;
        }

        public int CalculateAgility()
        {
            int value = Unit.base_agility;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_units)
                    value += research.ResearchInfo.agility_bonus;
            return value;
        }

        #region Queries
        public void QueryBattlegroup()
        {
            if (CivilizationInfo.unit_battlegroup_id == null)
                return;

            Debug.WriteLine(string.Format("Civilization Unit: {0}, Getting Battlegroups", CivilizationInfo.id));
            BattlegroupInfo = Database.Session.Query<DB_unit_battlegroups>().Where(x => x.id == CivilizationInfo.unit_battlegroup_id).First();
        }
        #endregion
    }
}
