﻿using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class GameIndex
    {
        public DB_users User { get; set; }
        public Game Game { get; set; }
    }
}
