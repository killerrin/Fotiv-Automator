using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class ViewJumpgate
    {
        public GamePlayer User { get; set; }
        public Jumpgate Jumpgate { get; set; }
    }
}
