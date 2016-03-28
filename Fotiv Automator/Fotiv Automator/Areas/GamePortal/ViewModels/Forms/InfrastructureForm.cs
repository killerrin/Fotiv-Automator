using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class InfrastructureForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int RPCost { get; set; }

        [Required]
        public bool IsColony { get; set; }
        [Required]
        public bool IsMilitary { get; set; }

        [Required]
        public int BaseHealth { get; set; }
        [Required]
        public int BaseAttack { get; set; }
        [Required]
        public int Influence { get; set; }

        [Required]
        public int RPBonus { get; set; }
        [Required]
        public int ScienceBonus { get; set; }
        [Required]
        public int ShipConstructionBonus { get; set; }
        [Required]
        public int ColonialDevelopmentBonus { get; set; }

        [Required]
        public bool ResearchSlot { get; set; }
        [Required]
        public bool ShipConstructionSlot { get; set; }
        [Required]
        public bool ColonialDevelopmentSlot { get; set; }

        public string GMNotes { get; set; }

        public IList<Checkbox> PossibleUpgrades { get; set; }

        public InfrastructureForm() { }
    }
}



