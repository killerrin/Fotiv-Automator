﻿using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels
{
    public class IndexTechLevels
    {
        public GamePlayer User { get; set; }
        public List<DB_tech_levels> TechLevels;
    }
}