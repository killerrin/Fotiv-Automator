using Fotiv_Automator.Infrastructure.Extensions;
using Fotiv_Automator.Models.StarMapGenerator.Models.Enums;
using Fotiv_Automator.Models.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fotiv_Automator.Models.StarMapGenerator.Models
{
    public abstract class CelestialBase : BindableBase
    {
        public List<SentientSpecies> Sentients { get; set; }

        public TerraformationTier TerraformingTier { get; set; }
        public LifeStage StageOfLife { get; set; }
        public int ResourceValue { get; set; }

        protected CelestialBase()
        {
            Sentients = new List<SentientSpecies>();
            ResourceValue = 0;
        }

        #region Individual Generators
        public static int GenerateNumberOfSentients(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0,80)) return 1;
            else if (percentage.IsBetween(80, 90)) return 2;
            else if (percentage.IsBetween(90, 95)) return die.Roll(1, 2) + 1;
            return die.Roll(2, 3);
        }
        public static TerraformationTier GenerateTerraformationTier(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 6)) return TerraformationTier.Uninhabitable;
            else if (percentage.IsBetween(6, 20)) return TerraformationTier.T1;
            else if (percentage.IsBetween(20, 35)) return TerraformationTier.T2;
            else if (percentage.IsBetween(35, 70)) return TerraformationTier.T3;
            else if (percentage.IsBetween(70, 85)) return TerraformationTier.T4;
            return TerraformationTier.T5;
        }
        public static LifeStage GenerateStageOfLife(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 10)) return LifeStage.NoLife;
            else if (percentage.IsBetween(10, 15)) return LifeStage.OrganicCompounds;
            else if (percentage.IsBetween(15, 25)) return LifeStage.SingleCellular;
            else if (percentage.IsBetween(25, 35)) return LifeStage.MultiCellular;
            else if (percentage.IsBetween(35, 60)) return LifeStage.SimpleLife;
            else if (percentage.IsBetween(60, 96)) return LifeStage.ComplexLife;
            return LifeStage.SentientLife;
        }
        public static LifeStage GenerateStageOfLifeT1T2(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage.IsBetween(0, 20)) return LifeStage.NoLife;
            else if (percentage.IsBetween(20, 30)) return LifeStage.OrganicCompounds;
            else if (percentage.IsBetween(30, 40)) return LifeStage.SingleCellular;
            else if (percentage.IsBetween(40, 75)) return LifeStage.MultiCellular;
            else if (percentage.IsBetween(75, 90)) return LifeStage.SimpleLife;
            return LifeStage.ComplexLife;
        }

        public static int GenerateResourceValue(Dice die)
        {
            return die.Roll(1, 10);
        }
        #endregion
    }
}
