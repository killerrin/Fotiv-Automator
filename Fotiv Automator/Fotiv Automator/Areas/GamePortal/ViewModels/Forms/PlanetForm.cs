
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
    public class PlanetForm
    {
        public int? ID { get; set; }

        [Required]
        public int StarID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Resources { get; set; }

        [Required]
        public bool SupportsColonies { get; set; }

        public string GMNotes { get; set; }

        [Required]
        public int? SelectedPlanetTier { get; set; }
        public IList<Checkbox> PlanetTiers { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedPlanetType { get; set; }
        public IList<Checkbox> PlanetTypes { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedStageOfLife { get; set; }
        public IList<Checkbox> StagesOfLife { get; set; } = new List<Checkbox>();
    }
}



