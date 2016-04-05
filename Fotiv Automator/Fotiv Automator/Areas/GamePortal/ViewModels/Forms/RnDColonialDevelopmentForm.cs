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
    public class RnDColonialDevelopmentForm
    {
        public int? ID { get; set; }

        [Required]
        public int? CivilizationID { get; set; }

        [Required]
        public int BuildPercentage { get; set; }

        [Required]
        public int? SelectedInfrastructure { get; set; }
        public IList<Checkbox> Infrastructure { get; set; } = new List<Checkbox>();
    }
}



