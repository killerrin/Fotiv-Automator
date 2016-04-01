﻿using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Jumpgate
    {
        public int JumpGateID { get { return Info.id; } }

        public DB_jumpgates Info;
        public Infrastructure Infrastructure;

        public Jumpgate(DB_jumpgates dbJumpgate)
        {
            Info = dbJumpgate;
        }
    }
}
