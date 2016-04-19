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
    public class Unit
    {
        public int ID { get { return Info.id; } }

        public DB_units Info;
        public DB_unit_categories UnitCategory;

        public Unit() { }
        public Unit(DB_units unit)
        {
            Info = unit;
        }
        public Unit(DB_units unit, DB_unit_categories category)
        {
            Info = unit;
            UnitCategory = category;
        }
    }
}
