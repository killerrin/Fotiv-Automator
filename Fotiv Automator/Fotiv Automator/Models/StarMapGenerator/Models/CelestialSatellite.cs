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
    public class CelestialSatellite : CelestialBase
    {
        public CelestialSatelliteType CelestialType { get; set; }
        public CelestialSatellite()
            :base()
        {
        }

        public bool SupportsColonies
        {
            get
            {
                return true;
            }
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
                //str += "\n\t\tSentients";
                str += "\n";
                foreach (var sentient in Sentients)
                {
                    str += string.Format("\t\t{0}\n", sentient);
                }
            }

            return str;
        }

        public static CelestialSatellite Generate(Dice die)
        {
            CelestialSatellite celestialSatellite = new CelestialSatellite();
            celestialSatellite.CelestialType = GenerateCelestialSatelliteType(die);
            celestialSatellite.TerraformingTier = GenerateTerraformationTier(die);

            if (celestialSatellite.TerraformingTier == TerraformationTier.Uninhabitable ||
                celestialSatellite.CelestialType == CelestialSatelliteType.GasCloud)
            {
                celestialSatellite.StageOfLife = LifeStage.NoLife;
            }
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T1 ||
                     celestialSatellite.TerraformingTier == TerraformationTier.T2)
            {
                celestialSatellite.StageOfLife = GenerateStageOfLifeT1T2(die);
            }
            else celestialSatellite.StageOfLife = GenerateStageOfLife(die);

            // Calculate Resource Value
            celestialSatellite.ResourceValue = GenerateResourceValue(die);
            if (celestialSatellite.TerraformingTier == TerraformationTier.Uninhabitable) celestialSatellite.ResourceValue += die.Roll(1, 10) + 10;
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T1) celestialSatellite.ResourceValue += die.Roll(1, 5);
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T2) celestialSatellite.ResourceValue += die.Roll(1, 3);
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T5) celestialSatellite.ResourceValue += die.Roll(1, 5) + 3;

            // Generate Sentient Species
            if (celestialSatellite.StageOfLife == LifeStage.SentientLife)
            {
                int numberOfSentients = GenerateNumberOfSentients(die);
                for (int i = 0; i < numberOfSentients; i++)
                {
                    celestialSatellite.Sentients.Add(SentientSpecies.Generate(die));
                }
            }

            return celestialSatellite;
        }

        #region Individual Generators
        public static CelestialSatelliteType GenerateCelestialSatelliteType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 45)) return CelestialSatelliteType.Moon;
            else if (percentage.IsBetween(45, 80)) return CelestialSatelliteType.Rings;
            else if (percentage.IsBetween(80, 95)) return CelestialSatelliteType.GasCloud;
            return CelestialSatelliteType.ArtificialBody;
        }
        #endregion
    }
}
