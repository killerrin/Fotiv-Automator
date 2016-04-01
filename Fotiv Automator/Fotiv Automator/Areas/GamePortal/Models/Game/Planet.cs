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
    public class Planet
    {
        public int PlanetID { get { return Info.id; } }

        public DB_planets Info;
        public DB_planet_tiers TierInfo;
        public List<Planet> Satellites = new List<Planet>();

        public Planet(DB_planets planet)
        {
            Info = planet;
            Satellites = new List<Planet>();
        }
    }
}
