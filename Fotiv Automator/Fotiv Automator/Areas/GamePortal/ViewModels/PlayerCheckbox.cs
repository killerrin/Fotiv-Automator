using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class PlayerCheckbox
    {
        public int PlayerID { get; set; }
        public string PlayerUsername { get; set; }
        public bool IsChecked { get; set; }

        public PlayerCheckbox() { }
        public PlayerCheckbox(int playerID, string playerUsername, bool isChecked)
        {
            PlayerID = playerID;
            PlayerUsername = playerUsername;
            IsChecked = isChecked;
        }
    }
}
