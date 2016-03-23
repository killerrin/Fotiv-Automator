using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class UserAccountUpdateForm
    {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";

        [DataType(DataType.Password), DisplayName("New Password")]
        public string NewPassword { get; set; } = "";

        [DataType(DataType.Password), DisplayName("Again")]
        public string VerifyNewPassword { get; set; } = "";

        [Required, DataType(DataType.Password), DisplayName("Current Password"),]
        public string CurrentPassword { get; set; } = "";
    }
}
