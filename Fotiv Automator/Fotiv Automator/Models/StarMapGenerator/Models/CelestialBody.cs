using Fotiv_Automator.Infrastructure.Extensions;
using Fotiv_Automator.Models.StarMapGenerator.Models.Enums;
using Fotiv_Automator.Models.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.StarMapGenerator.Models
{
    public class CelestialBody : CelestialBase
    {
        public List<CelestialSatellite> OrbitingSatellites { get; set; }
        public CelestialBodyType CelestialType { get; set; }

        public int TotalResources
        {
            get
            {
                int resources = ResourceValue;
                foreach (var satellite in OrbitingSatellites)
                    resources += satellite.ResourceValue;
                return resources;
            }
        }

        public bool SupportsColonies
        {
            get
            {
                if (CelestialType == CelestialBodyType.Blackhole ||
                    CelestialType == CelestialBodyType.SubStar ||
                    CelestialType == CelestialBodyType.Comet ||
                    CelestialType == CelestialBodyType.Wormhole)
                    return false;
                return true;
            }
        }

        public CelestialBody()
            :base()
        {
            OrbitingSatellites = new List<CelestialSatellite>();
        }

        public override string ToString()
        {
            string str = string.Format("{0}RP\t{1}\t{2}\t{3}",
                ResourceValue,
                StringExtensions.SpaceUppercaseLetters(CelestialType.ToString()),
                StringExtensions.SpaceUppercaseLetters(TerraformingTier.ToString()),
                StringExtensions.SpaceUppercaseLetters(StageOfLife.ToString()));

            if (Sentients.Count > 0)
            {
                //str += "\n\tSentients";
                str += "\n";
                foreach (var sentient in Sentients)
                {
                    str += string.Format("\t{0}\n", sentient);
                }
            }

            if (OrbitingSatellites.Count > 0)
            {
                //str += "\n\tSatellites";
                str += "\n";
                foreach (var satellite in OrbitingSatellites)
                {
                    str += string.Format("\t{0}\n", satellite);
                }
            }

            return str;
        }

        public static CelestialBody Generate(Dice die)
        {
            CelestialBody celestialObject = new CelestialBody();
            celestialObject.CelestialType = GenerateCelestialBodyType(die);

            // Terraforming Tier
            if (celestialObject.CelestialType == CelestialBodyType.Blackhole ||
                celestialObject.CelestialType == CelestialBodyType.SubStar ||
                celestialObject.CelestialType == CelestialBodyType.Wormhole ||
                celestialObject.CelestialType == CelestialBodyType.Comet)
            {
                celestialObject.TerraformingTier = TerraformationTier.Uninhabitable;
            }
            else celestialObject.TerraformingTier = GenerateTerraformationTier(die);

            // Stage of Life
            if (celestialObject.TerraformingTier == TerraformationTier.Uninhabitable ||
                celestialObject.CelestialType == CelestialBodyType.Blackhole ||
                celestialObject.CelestialType == CelestialBodyType.SubStar ||
                celestialObject.CelestialType == CelestialBodyType.Wormhole ||
                celestialObject.CelestialType == CelestialBodyType.Comet)
            {
                celestialObject.StageOfLife = LifeStage.NoLife;
            }
            else if (celestialObject.TerraformingTier == TerraformationTier.T1 ||
                     celestialObject.TerraformingTier == TerraformationTier.T2 ||
                     celestialObject.CelestialType == CelestialBodyType.GasPlanet)
            {
                celestialObject.StageOfLife = GenerateStageOfLifeT1T2(die);
            }
            else celestialObject.StageOfLife = GenerateStageOfLife(die);

            // Calculate Resource Value
            celestialObject.ResourceValue = GenerateResourceValue(die);
            if (celestialObject.CelestialType == CelestialBodyType.Blackhole || celestialObject.CelestialType == CelestialBodyType.Wormhole)
            {
                celestialObject.ResourceValue = 0;
            }
            else if (celestialObject.TerraformingTier == TerraformationTier.Uninhabitable) celestialObject.ResourceValue += die.Roll(1, 10) + 10;
            else if (celestialObject.TerraformingTier == TerraformationTier.T1) celestialObject.ResourceValue += die.Roll(1, 5);
            else if (celestialObject.TerraformingTier == TerraformationTier.T2) celestialObject.ResourceValue += die.Roll(1, 3);
            else if (celestialObject.TerraformingTier == TerraformationTier.T5) celestialObject.ResourceValue += die.Roll(1, 5) + 3;

            // Generate Sentient Species
            if (celestialObject.StageOfLife == LifeStage.SentientLife)
            {
                int numberOfSentients = GenerateNumberOfSentients(die);
                for (int i = 0; i < numberOfSentients; i++)
                {
                    celestialObject.Sentients.Add(SentientSpecies.Generate(die));
                }
            }

            // Generate Celestial Satellites
            if (celestialObject.CelestialType == CelestialBodyType.Comet) { }
            else if (celestialObject.CelestialType == CelestialBodyType.Wormhole) { }
            //else if (celestialObject.CelestialType == CelestialBodyType.AsteroidBelt) { }
            else
            {
                int numberOfCelestialSatellites = GenerateNumberOfCelestialSatellites(die);
                for (int i = 0; i < numberOfCelestialSatellites; i++)
                {
                    celestialObject.OrbitingSatellites.Add(CelestialSatellite.Generate(die));
                }
            }

            return celestialObject;
        }

        #region Individual Generators
        public static int GenerateNumberOfCelestialSatellites(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 60)) return 1;
            else if (percentage.IsBetween(60, 80)) return 2;
            else if (percentage.IsBetween(80, 95)) return die.Roll(1, 4);
            return die.Roll(2, 4);
        }

        public static CelestialBodyType GenerateCelestialBodyType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 7)) return CelestialBodyType.Blackhole;
            else if (percentage.IsBetween(7, 15)) return CelestialBodyType.SubStar;
            else if (percentage.IsBetween(15, 25)) return CelestialBodyType.Comet;
            else if (percentage.IsBetween(25, 40)) return CelestialBodyType.AsteroidBelt;
            else if (percentage.IsBetween(40, 65)) return CelestialBodyType.GasPlanet;
            else if (percentage.IsBetween(65, 90)) return CelestialBodyType.TerrestrialPlanet;
            return CelestialBodyType.ArtificialPlanet;
        }
        #endregion
    }
}
