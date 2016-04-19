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
    public class CivilizationUnitsForm
    {
        public int? ID { get; set; }

        [Required]
        public int? CivilizationID { get; set; }

        [Required]
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int Experience { get; set; }

        public string GMNotes { get; set; }

        [Required]
        public int? SelectedUnitID { get; set; }
        public IList<Checkbox> Units { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedSpeciesID { get; set; }
        public IList<Checkbox> Species { get; set; } = new List<Checkbox>();
    }
}



