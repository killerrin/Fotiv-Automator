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
    public class ResearchForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int RPCost { get; set; }

        [Required]
        public int HealthBonus { get; set; }
        [Required]
        public int AttackBonus { get; set; }

        [Required]
        public int ScienceBonus { get; set; }
        [Required]
        public int ColonialDevelopmentBonus { get; set; }
        [Required]
        public int ShipConstructionBonus { get; set; }

        public string GMNotes { get; set; }

    }
}



