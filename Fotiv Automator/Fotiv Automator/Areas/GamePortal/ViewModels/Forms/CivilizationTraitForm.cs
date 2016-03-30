using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class CivilizationTraitForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int LocalInfluenceBonus { get; set; }
        [Required]
        public int ForeignInfluenceBonus { get; set; }
        [Required]
        public int TradeBonus { get; set; }

        [Required]
        public bool ApplyMilitary { get; set; }
        [Required]
        public bool ApplyUnits { get; set; }
        [Required]
        public bool ApplyShips { get; set; }
        [Required]
        public bool ApplyInfrastructure { get; set; }

        [Required]
        public int ScienceBonus { get; set; }
        [Required]
        public int ColonialDevelopmentBonus { get; set; }
        [Required]
        public int ShipConstructionBonus { get; set; }
        [Required]
        public int UnitTrainingBonus { get; set; }
    }
}



