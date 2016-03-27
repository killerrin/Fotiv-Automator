using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Research
    {
        public DB_civilization_research CivilizationInfo;
        public DB_research ResearchInfo;

        public Research(DB_civilization_research info)
        {
            CivilizationInfo = info;
        }
    }
}
