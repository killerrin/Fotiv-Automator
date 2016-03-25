using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class ViewCivilization
    {
        public int GameID { get; set; }

        public GamePlayer User { get; set; }
        public Civilization Civilization { get; set; }
    }
}
