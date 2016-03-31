using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.Tools;
using NHibernate.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Sector
    {
        public DB_sectors Info;

        public List<Starsystem> StarSystemsRaw;
        public List<List<Starsystem>> StarSystems;

        public Sector(DB_sectors sector)
        {
            Info = sector;

            QueryAllStarsystems();
        }

        public Starsystem StarsystemFromHex(HexCoordinate hex)
        {
            return StarSystemsRaw
                .Where(x => x.Info.hex_x == hex.X)
                .Where(y => y.Info.hex_y == hex.Y)
                .First();
        }

        #region Querys
        public void QueryAllStarsystems()
        {
            Debug.WriteLine(string.Format("Sector: {0}, Getting Star Systems", Info.id));
            var dbSystems = Database.Session.Query<DB_starsystems>()
                .Where(x => x.sector_id == Info.id)
                .ToList();

            StarSystemsRaw = new List<Starsystem>();
            foreach (var system in dbSystems)
                StarSystemsRaw.Add(new Starsystem(system));


            var sortingList = new List<Starsystem>(StarSystemsRaw);
            StarSystems = new List<List<Starsystem>>();
            while (sortingList.Count > 0)
            {
                var newColumn = new List<Starsystem>();

                int currentX = StarSystems.Count;
                int currentY = 0;
                for(int i = sortingList.Count - 1; i >= 0; i--)
                {
                    if (sortingList[i].HexCode.IsCoordinate(currentX, currentY))
                    {
                        newColumn.Add(sortingList[i]);
                        currentY++;

                        sortingList.RemoveAt(i);
                        continue;
                    }
                }

                if (newColumn.Count > 0) StarSystems.Add(newColumn);
            }
        }
        #endregion
    }
}
