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
    public class ShipForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int RPCost { get; set; }

        [Required]
        public bool IsMilitary { get; set; }
        [Required]
        public bool IsFighter { get; set; }

        [Required]
        public int BaseHealth { get; set; } = 1;
        [Required]
        public int BaseRegeneration { get; set; }

        [Required]
        public int BaseAttack { get; set; } = 1;
        [Required]
        public int BaseSpecialAttack { get; set; }

        [Required]
        public int BaseAgility { get; set; }

        [Required]
        public int MaximumFighters { get; set; }
        [Required]
        public int NumBuild { get; set; } = 1;

        public string GMNotes { get; set; }

        [Required]
        public int? SelectedShipRate { get; set; }
        public IList<Checkbox> ShipRates { get; set; } = new List<Checkbox>();
    }
}



