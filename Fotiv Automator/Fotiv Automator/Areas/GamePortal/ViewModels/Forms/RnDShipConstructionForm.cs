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
    public class RnDShipConstructionForm
    {
        public int? ID { get; set; }

        [Required]
        public int? CivilizationID { get; set; }

        [Required]
        public int HexX { get; set; }
        [Required]
        public int HexY { get; set; }

        [Required]
        public int BuildPercentage { get; set; }

        public int CurrentHealth { get; set; }
        public bool CommandAndControl { get; set; }
        public string GMNotes { get; set; }

        [Required]
        public int? SelectedShipID { get; set; }
        public IList<Checkbox> Ships { get; set; } = new List<Checkbox>();


    }
}



