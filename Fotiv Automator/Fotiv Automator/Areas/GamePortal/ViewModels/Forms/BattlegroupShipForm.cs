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
    public class BattlegroupShipForm
    {
        public int? ID { get; set; }

        [Required]
        public int? CivilizationID { get; set; }

        [Required]
        public int HexX { get; set; }
        [Required]
        public int HexY { get; set; }

        [Required]
        public string Name { get; set; }
        public string GMNotes { get; set; }

        public IList<Checkbox> UnassignedShips { get; set; } = new List<Checkbox>();
        public IList<Checkbox> BattlegroupShips { get; set; } = new List<Checkbox>();
    }
}



