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
        public int ID { get { return Info.id; } }

        public DB_sectors Info;

        public List<Starsystem> StarSystemsRaw = new List<Starsystem>();
        public List<List<Starsystem>> StarSystems = new List<List<Starsystem>>();

        public int MaxX { get { return StarSystems.Count; } }
        public int MaxY
        {
            get
            {
                if (MaxX > 0)
                    return StarSystems[0].Count;
                return 0;
            }
        }

        public Sector(DB_sectors sector)
        {
            Info = sector;
        }

        public Starsystem StarsystemFromHex(HexCoordinate hex)
        {
            return StarSystemsRaw
                .Where(x => x.Info.hex_x == hex.X)
                .Where(y => y.Info.hex_y == hex.Y)
                .First();
        }
    }
}
