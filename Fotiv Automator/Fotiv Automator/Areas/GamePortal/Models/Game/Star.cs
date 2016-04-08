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
    public class Star
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

        public DB_stars Info;
        public List<Planet> Planets = new List<Planet>();

        public Star(DB_stars star)
        {
            Info = star;
        }
    }
}
