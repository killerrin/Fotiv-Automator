using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class BattlegroupShip
    {
        public int ID { get { return Info.id; } }
        public DB_ship_battlegroups Info;

        public List<CivilizationShip> Ships;

        public BattlegroupShip(DB_ship_battlegroups info)
        {
            Info = info;
            Ships = new List<CivilizationShip>();
        }

        public int CalculateCurrentHealth()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CivilizationInfo.current_health;
            return value;
        }

        public int CalculateMaxHealth()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CalculateMaxHealth();
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CalculateRegenerationFactor();
            return value;
        }

        public int CalculateAttack()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CalculateAttack();
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CalculateSpecialAttack();
            return value;
        }

        public int CalculateAgility()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.CalculateAgility();
            return value;
        }

        public int CalculateCurrentFighters()
        {
            int value = 0;
            foreach (var ship in Ships)
                if (ship.Ship.Info.is_fighter)
                    value++;
            return value;
        }

        public int CalculateMaxFighterSlots()
        {
            int value = 0;
            foreach (var ship in Ships)
                value += ship.Ship.Info.maximum_fighters;
            return value;
        }
    }
}
