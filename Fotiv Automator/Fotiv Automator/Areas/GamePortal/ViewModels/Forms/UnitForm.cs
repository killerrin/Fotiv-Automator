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
    public class UnitForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int RPCost { get; set; }
        [Required]
        public int NumberToBuild { get; set; } = 1;

        [Required]
        public bool CanEmbark { get; set; }
        [Required]
        public bool CanAttackGroundUnits { get; set; }
        [Required]
        public bool CanAttackBoats { get; set; }
        [Required]
        public bool CanAttackPlanes { get; set; }
        [Required]
        public bool CanAttackSpaceships { get; set; }

        [Required]
        public int EmbarkingSlots { get; set; }
        [Required]
        public int NegateDamage { get; set; }

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

        public string GMNotes { get; set; }

        [Required]
        public int? SelectedUnitTypeID { get; set; }
        public IList<Checkbox> UnitTypes { get; set; } = new List<Checkbox>();

        [Required]
        public int? SelectedCategoryID { get; set; }
        public IList<Checkbox> Categories { get; set; } = new List<Checkbox>();
    }
}



