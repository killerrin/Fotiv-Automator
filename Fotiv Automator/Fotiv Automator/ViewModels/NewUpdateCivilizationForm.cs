using Fotiv_Automator.Models.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class NewUpdateCivilizationForm
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

        public IList<PlayerCheckbox> Players { get; set; }
    }
}
