
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
    public class StarForm
    {
        public int? ID { get; set; }

        [Required]
        public int StarsystemID { get; set; }

        [Required]
        public string Name { get; set; }
        public string GMNotes { get; set; }

        [Required]
        public int? SelectedStarType { get; set; }
        public IList<Checkbox> StarTypes { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedStarAge { get; set; }
        public IList<Checkbox> StarAges { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedRadiationLevel { get; set; }
        public IList<Checkbox> RadiationLevels { get; set; } = new List<Checkbox>();
    }
}



