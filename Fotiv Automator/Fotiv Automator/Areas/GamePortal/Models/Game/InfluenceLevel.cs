using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class InfluenceLevel
    {
        public Civilization Civilization;
        public float Influence;

        public InfluenceLevel(Civilization civ, int influence)
        {
            Civilization = civ;
            Influence = influence;
        }
    }
}
