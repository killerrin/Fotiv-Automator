
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
    public class StarsystemForm
    {
        public int? ID { get; set; }
        public string GMNotes { get; set; }

        public List<Checkbox> CivilizationVisited { get; set; }
    }
}



