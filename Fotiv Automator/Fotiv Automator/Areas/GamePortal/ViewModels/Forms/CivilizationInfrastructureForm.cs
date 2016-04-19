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
    public class CivilizationInfrastructureForm
    {
        public int? ID { get; set; }

        [Required]
        public int? CivilizationID { get; set; }

        [Required]
        public int? PlanetID { get; set; }

        [Required]
        public string Name { get; set; }

        public int CurrentHealth { get; set; }

        public bool CanUpgrade { get; set; }
        public bool IsMilitary { get; set; }
        public string GMNotes { get; set; }

        [Required]
        public int? SelectedInfrastructureID { get; set; }
        public IList<Checkbox> Infrastructure { get; set; } = new List<Checkbox>();
    }
}



