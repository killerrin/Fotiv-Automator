﻿using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Wormhole
    {
        public int ID { get { return Info.id; } }
        public DB_wormholes Info;

        public Starsystem SystemOne { get; set; }
        public Starsystem SystemTwo { get; set; }

        public Wormhole(DB_wormholes dbWormhole)
        {
            Info = dbWormhole;
        }
    }
}