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
    public class Planet : IBBCodeFormatter
    {
        public int PlanetID { get { return Info.id; } }

        public DB_planets Info;
        public DB_planet_tiers TierInfo;
        public DB_planet_types TypeInfo;
        public DB_stage_of_life StageOfLifeInfo;

        public Star Star;
        public List<Planet> Satellites = new List<Planet>();
        public List<CivilizationInfrastructure> Infrastructure = new List<CivilizationInfrastructure>();

        public bool HasSatellites { get { return Satellites.Count > 0; } }
        public bool HasInfrastructure { get { return Infrastructure.Count > 0; } }

        public Planet(DB_planets planet)
        {
            Info = planet;
            Satellites = new List<Planet>();
            Infrastructure = new List<Models.Game.CivilizationInfrastructure>();
        }

        public string ToBBCode()
        {
            string planetName = (Info.name == TypeInfo.name) ? "" : $"{Info.name} \t";

            BBCodeWriter bbCodeWriter = new BBCodeWriter();
            bbCodeWriter.Append($"{planetName} {TypeInfo.name} \t {TierInfo.name} \t {StageOfLifeInfo.name}");
            foreach (var satellite in Satellites) bbCodeWriter.Append($"\n\t {satellite.ToBBCode()}");

            return bbCodeWriter.ToString();
        }
    }
}
