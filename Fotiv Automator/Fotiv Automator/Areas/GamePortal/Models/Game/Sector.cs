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

        #region ID Search
        public Starsystem StarsystemFromHex(int hexX, int hexY) { return StarsystemFromHex(new HexCoordinate(hexX, hexY)); }
        public Starsystem StarsystemFromHex(HexCoordinate hex)
        {
            Debug.WriteLine("StarsystemFromHex");

            foreach (var system in StarSystemsRaw)
            {
                if (system.HexCode == hex)
                    return system;
            }

            return null;
        }

        public Starsystem StarsystemFromID(int id)
        {
            foreach (var system in StarSystemsRaw)
                if (system.ID == id)
                    return system;
            return null;
        }

        public Wormhole WormholeFromID(int id)
        {
            foreach (var system in StarSystemsRaw)
                foreach (var wormhole in system.Wormholes)
                    if (wormhole.ID == id)
                        return wormhole;
            return null;
        }

        public Jumpgate JumpgateFromID(int id)
        {
            foreach (var system in StarSystemsRaw)
                foreach (var jumpgate in system.Jumpgates)
                    if (jumpgate.ID == id)
                        return jumpgate;
            return null;
        }

        public Star StarFromID(int id)
        {
            foreach (var system in StarSystemsRaw)
                foreach (var star in system.Stars)
                    if (star.ID == id)
                        return star;
            return null;
        }

        public Planet PlanetFromID(int id)
        {
            foreach (var system in StarSystemsRaw)
                foreach (var star in system.Stars)
                    foreach (var planet in star.Planets)
                        if (planet.PlanetID == id)
                            return planet;
            return null;
        }
        #endregion

        public const int MAX_INFLUENCE_RINGS = 3;
        public List<InfluenceLevel> CalculateInflueceForSystem(HexCoordinate coordinate)
        {
            // First things first, calculate all of the systems within our distance
            var systemRings = new List<Starsystem>();
            foreach (var system in StarSystemsRaw)
            {
                if (system.HexCode.WithinDistance(coordinate, MAX_INFLUENCE_RINGS))
                {
                    systemRings.Add(system);
                }
            }

            // Now, calculate the Influence of all the infrastructures
            var influenceLevels = new List<InfluenceLevel>();
            foreach (var system in systemRings)
            {
                foreach (var star in system.Stars)
                {
                    foreach (var planet in star.Planets)
                    {
                        foreach (var infrastructure in planet.Infrastructure)
                        {
                            InfluenceLevel level = influenceLevels.Where(x => x.Civilization.ID == infrastructure.CivilizationID).FirstOrDefault();
                            if (level == null)
                            {
                                level = new InfluenceLevel(infrastructure.Owner, 0);
                                influenceLevels.Add(level);
                            }

                            float influence = infrastructure.InfrastructureInfo.Infrastructure.influence_bonus;
                            for (int i = 0; i < coordinate.Distance(system.HexCode); i++)
                                influence /= 2;

                            level.Influence += influence;
                        }
                    }
                }
            }

            foreach (var level in influenceLevels)
            {
                if (level.Civilization.CivilizationTrait1 != null)
                    level.Influence += level.Civilization.CivilizationTrait1.influence_bonus;

                if (level.Civilization.CivilizationTrait2 != null)
                    level.Influence += level.Civilization.CivilizationTrait2.influence_bonus;

                if (level.Civilization.CivilizationTrait3 != null)
                    level.Influence += level.Civilization.CivilizationTrait3.influence_bonus;
            }
            return influenceLevels;
        }
    }
}
