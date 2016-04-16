using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class BattlegroupUnit
    {
        public int ID { get { return Info.id; } }
        public DB_unit_battlegroups Info;

        public List<CivilizationUnit> Units;

        public BattlegroupUnit(DB_unit_battlegroups info)
        {
            Info = info;
            Units = new List<CivilizationUnit>();
        }

        public int CalculateCurrentHealth()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CivilizationInfo.current_health;
            return value;
        }

        public int CalculateMaxHealth()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CalculateMaxHealth();
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CalculateRegenerationFactor();
            return value;
        }

        public int CalculateAttack()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CalculateAttack();
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CalculateSpecialAttack();
            return value;
        }

        public int CalculateAgility()
        {
            int value = 0;
            foreach (var ship in Units)
                value += ship.CalculateAgility();
            return value;
        }
    }
}
