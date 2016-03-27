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
    public class Ship
    {
        public DB_ships Info;
        public DB_ship_rates ShipRate;

        public Ship() { }
        public Ship(DB_ships ship)
        {
            Info = ship;
        }
        public Ship(DB_ships ship, DB_ship_rates rate)
        {
            Info = ship;
            ShipRate = rate;
        }
    }
}
