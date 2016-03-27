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
        public DB_stars Info;
        public List<Planet> Planets;

        public Star(DB_stars star)
        {
            Info = star;

            QueryAllPlanets();
        }

        public void QueryAllPlanets()
        {
            Planets = new List<Planet>();

            Debug.WriteLine(string.Format("Star: {0}, Getting Planets", Info.id));
            var dbPlanets = Database.Session.Query<DB_planets>()
                .Where(x => x.star_id == Info.id)
                .ToList();

            foreach (var dbPlanet in dbPlanets)
                Planets.Add(new Planet(dbPlanet));
        }
    }
}
