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
    public class CivilizationForm
    {
        public Game Game { get; set; }
        public int CivilizationID { get; set; } = -1;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public int RP { get; set; }

        public string Notes { get; set; }

        [DisplayName("GM Notes")]
        public string GMNotes { get; set; }

        public IList<Checkbox> Players { get; set; }
    }
}
