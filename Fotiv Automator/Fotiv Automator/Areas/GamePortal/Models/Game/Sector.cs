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
        public const int HEX_RADIUS = 30;
        public int HexRadius { get { return HEX_RADIUS; } }
        public int HexSize { get { return HEX_RADIUS * 2; } }

        public int ID { get { return Info.id; } }

        public DB_sectors Info;

        public List<Starsystem> StarSystemsRaw = new List<Starsystem>();
        public List<List<Starsystem>> StarSystems = new List<List<Starsystem>>();

        public int Width { get { return StarSystems.Count; } }
        public int Height
        {
            get
            {
                if (Width > 0)
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
            Debug.WriteLine("StarsystemFromHex");

            foreach (var system in StarSystemsRaw)
            {
                if (system.HexCode.X == hex.X &&
                    system.HexCode.Y == hex.Y)
                    return system;
            }

            return null;
        }
    }
}
