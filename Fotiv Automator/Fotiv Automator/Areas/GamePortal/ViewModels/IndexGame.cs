﻿using Fotiv_Automator.Areas.GamePortal.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class IndexGame
    {
        public int GameID { get; set; }

        public GamePlayer User { get; set; }
        public Game Game { get; set; }
    }
}