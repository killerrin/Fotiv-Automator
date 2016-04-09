using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class SectorForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("GM Notes")]
        public string GMNotes { get; set; }


        public int Width { get; set; }
        public int Height { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUpload { get; set; }
    }
}
