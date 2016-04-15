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
    }
}
