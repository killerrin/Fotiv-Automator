
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
    public class WormholeForm
    {
        public int? ID { get; set; }

        [Required]
        public int StarsystemID { get; set; }

        public string GMNotes { get; set; }

        [Required]
        public int HexX;
        [Required]
        public int HexY;
    }
}



