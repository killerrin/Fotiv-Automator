using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Battlegroup
    {
        public int ID { get { return Info.id; } }
        public DB_civilization_battlegroups Info;

        public List<CivilizationUnit> Units;
        public int TotalUnits { get { return Units.Count; } }

        public Battlegroup(DB_civilization_battlegroups info)
        {
            Info = info;
            Units = new List<CivilizationUnit>();
        }

        public int CalculateCurrentHealth()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CivilizationInfo.current_health;
            return value;
        }

        public int CalculateMaxHealth()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CalculateMaxHealth();
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CalculateRegenerationFactor();
            return value;
        }

        public int CalculateAttack()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CalculateAttack();
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CalculateSpecialAttack();
            return value;
        }

        public int CalculateAgility()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.CalculateAgility();
            return value;
        }

        public int CalculateCurrentEmbarked()
        {
            int value = 0;
            foreach (var unit in Units)
                if (unit.Unit.Info.can_embark)
                    value++;
            return value;
        }

        public int CalculateMaxEmbarked()
        {
            int value = 0;
            foreach (var unit in Units)
                value += unit.Unit.Info.embarking_slots;
            return value;
        }
    }
}
