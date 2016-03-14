using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class Sector
    {
        public DB_sectors Info;
        public DB_game_sectors GameSectorInfo;

        public List<Starsystem> Starsystems;

        public Sector(DB_sectors sector, DB_game_sectors gameSector)
        {
            Info = sector;
            GameSectorInfo = gameSector;

            QueryAllStarsystems();
        }

        public void QueryAllStarsystems()
        {
            Starsystems = new List<Starsystem>();

            Debug.WriteLine(string.Format("Sector: {0}, Getting Star Systems", Info.id));
            var dbSystems = Database.Session.Query<DB_starsystems>()
                .Where(x => x.sector_id == Info.id)
                .ToList();

            foreach (var system in dbSystems)
                Starsystems.Add(new Starsystem(system));
        }
    }
}
