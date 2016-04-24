using Fotiv_Automator.Infrastructure;
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
    public class Starsystem : IBBCodeFormatter
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
        public List<Wormhole> Wormholes = new List<Wormhole>();

        public Starsystem(DB_starsystems system)
        {
            Info = system;
            HexCode = new HexCoordinate(Info.hex_x, Info.hex_y);
        }

        public bool InfrastructureInSystem()
        {
            foreach (var star in Stars)
            {
                foreach (var planet in star.Planets)
                {
                    if (planet.HasInfrastructure)
                        return true;
                }
            }

            return false;
        }

        public string ToBBCode()
        {
            BBCodeWriter bbCodeWriter = new BBCodeWriter();
            bbCodeWriter.BeginTag("spoiler", false, new BBCodeParameter("System"));
            bbCodeWriter.AppendLine($"Hex: {HexCode} \t Total Resources: {TotalResources}");
            bbCodeWriter.AppendLine();
            bbCodeWriter.AppendLine("b", "Stars");
            foreach (var star in Stars) bbCodeWriter.AppendLine(star.ToBBCode());
            bbCodeWriter.AppendLine("b", "Wormholes");
            foreach (var wormhole in Wormholes) bbCodeWriter.AppendLine(wormhole.ToBBCode());
            bbCodeWriter.EndTag(true);

            return bbCodeWriter.ToString();
        } 
    }
}
