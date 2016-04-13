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
        public DB_planet_types TypeInfo;
        public DB_stage_of_life StageOfLifeInfo;

        public Star Star;
        public List<Planet> Satellites = new List<Planet>();
        public List<Infrastructure> Infrastructure = new List<Infrastructure>();

        public bool HasSatellites { get { return Satellites.Count > 0; } }
        public bool HasInfrastructure { get { return Infrastructure.Count > 0; } }

        public Planet(DB_planets planet)
        {
            Info = planet;
            Satellites = new List<Planet>();
            Infrastructure = new List<Models.Game.Infrastructure>();
        }
    }
}
