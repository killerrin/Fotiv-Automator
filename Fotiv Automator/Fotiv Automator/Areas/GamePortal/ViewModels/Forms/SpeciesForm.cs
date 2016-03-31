using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class SpeciesForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int BaseAttack { get; set; }
        [Required]
        public int BaseSpecialAttack { get; set; }
        [Required]
        public int BaseHealth { get; set; }
        [Required]
        public int BaseRegeneration { get; set; }
        [Required]
        public int BaseAgility { get; set; }

        [DisplayName("GM Notes")]
        public string GMNotes { get; set; }

        public IList<Checkbox> Civilizations { get; set; } = new List<Checkbox>();
    }
}
