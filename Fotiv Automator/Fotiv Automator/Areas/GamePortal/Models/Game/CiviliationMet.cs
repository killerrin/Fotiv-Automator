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
    public class CivilizationMet
    {
        public int ID { get { return Info.id; } }
        public DB_civilization_met Info;

        public Civilization CivilizationOne;
        public Civilization CivilizationTwo;

        public CivilizationMet(DB_civilization_met dbCivilizationMet)
        {
            Info = dbCivilizationMet;
        }

    }
}
