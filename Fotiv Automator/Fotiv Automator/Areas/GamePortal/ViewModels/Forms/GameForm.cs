using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class GameForm
    {
        public int? GameID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int TurnNumber { get; set; }

        [Required]
        public bool OpenedToPublic { get; set; }

        public IList<Checkbox> PotentialGMs { get; set; } = new List<Checkbox>();
    }
}



