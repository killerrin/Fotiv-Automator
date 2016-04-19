using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class ViewCivilizationInfrastructure
    {
        public GamePlayer User { get; set; }
        public Models.Game.Infrastructure Infrastructure { get; set; }

        public bool PlayerOwnsCivilization { get; set; }
    }
}
