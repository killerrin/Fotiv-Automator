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
    public class ExperienceLevelForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Threshold { get; set; }

        [Required]
        public int HealthBonus { get; set; }
        [Required]
        public int RegenerationBonus { get; set; }
        [Required]
        public int AttackBonus { get; set; }
        [Required]
        public int SpecialAttackBonus { get; set; }
        [Required]
        public int AgilityBonus { get; set; }
    }
}



