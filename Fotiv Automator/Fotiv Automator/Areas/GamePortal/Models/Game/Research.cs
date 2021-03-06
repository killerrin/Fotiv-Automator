﻿using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Research
    {
        public int ResearchID { get { return ResearchInfo.id; } }

        public DB_civilization_research CivilizationInfo;
        public DB_research ResearchInfo;

        public Civilization Owner;

        public Research(DB_civilization_research info, Civilization owner)
        {
            CivilizationInfo = info;
            Owner = owner;
        }
    }
}
