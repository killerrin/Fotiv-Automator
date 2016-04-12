
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
        public string StageOfLife { get; set; }

        [Required]
        public int Resources { get; set; }

        [Required]
        public bool SupportsColonies { get; set; }

        public string GMNotes { get; set; }

        //public virtual int? orbiting_planet_id { get; set; }
        //public virtual int? planet_tier_id { get; set; }
    }
}



