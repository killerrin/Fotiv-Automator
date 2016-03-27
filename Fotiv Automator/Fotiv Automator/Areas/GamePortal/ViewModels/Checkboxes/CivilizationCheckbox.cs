using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes
{
    public class CivilizationCheckbox
    {
        public int CivilizationID { get; set; }
        public string CivilizationName { get; set; }
        public bool IsChecked { get; set; }

        public CivilizationCheckbox() { }
        public CivilizationCheckbox(int civilizationID, string civilizationName, bool isChecked)
        {
            CivilizationID = civilizationID;
            CivilizationName = civilizationName;
            IsChecked = isChecked;
        }
    }
}
