using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class ViewBattlegroup
    {
        public GamePlayer User { get; set; }
        public Battlegroup Battlegroup { get; set; }

        public bool PlayerOwnsCivilization { get; set; }
    }
}
