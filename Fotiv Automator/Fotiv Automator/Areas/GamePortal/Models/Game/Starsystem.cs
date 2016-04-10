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
    public class Starsystem
    {
        public int ID { get { return Info.id; } }
        public HexCoordinate HexCode { get; protected set; }
        public int TotalResources
        {
            get
            {
                int total = 0;
                foreach (var star in Stars)
                    total += star.TotalResources;
                return total;
            }
        }

        public Sector Sector;

        public DB_starsystems Info;
        public List<Star> Stars = new List<Star>();
        public List<Jumpgate> Jumpgates = new List<Jumpgate>();
        public List<DB_wormholes> WormholeInfos = new List<DB_wormholes>();

        public Starsystem(DB_starsystems system)
        {
            Info = system;
            HexCode = new HexCoordinate(Info.hex_x, Info.hex_y);
        }
    }
}
