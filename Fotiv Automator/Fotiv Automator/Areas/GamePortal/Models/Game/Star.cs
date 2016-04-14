using Fotiv_Automator.Infrastructure;
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
    public class Star : IBBCodeFormatter
    {
        public int ID { get { return Info.id; } }
        public int TotalResources
        {
            get
            {
                int total = 0;
                foreach (var planet in Planets)
                    total += planet.Info.resources;
                return total;
            }
        }

        public Starsystem SolarSystem;

        public DB_stars Info;
        public DB_star_types StarTypeInfo;
        public DB_star_ages StarAgeInfo;
        public DB_radiation_levels RadiationLevelInfo;

        public List<Planet> Planets = new List<Planet>();

        public Star(DB_stars star)
        {
            Info = star;
        }

        public string ToBBCode()
        {
            string starName = (Info.name == StarTypeInfo.name) ? "" : $"{Info.name} \t";

            BBCodeWriter bbCodeWriter = new BBCodeWriter();
            bbCodeWriter.AppendLine($"{starName} {StarTypeInfo.name} \t {StarAgeInfo.name} \t {RadiationLevelInfo.name}");
            foreach (var planet in Planets)
                if (planet.Info.orbiting_planet_id == null)
                    bbCodeWriter.Append($"\n{planet.ToBBCode()}");

            bbCodeWriter.AppendLine();
            return bbCodeWriter.ToString();
        }
    }
}
