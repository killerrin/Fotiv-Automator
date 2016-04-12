using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class IndexPlanets
    {
        public GamePlayer User { get; set; }
        public List<Planet> Planets { get; set; }

        public bool SatelliteMode { get; set; } = false;
    }
}
