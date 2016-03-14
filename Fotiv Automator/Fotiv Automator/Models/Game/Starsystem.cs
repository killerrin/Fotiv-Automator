﻿using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class Starsystem
    {
        public DB_starsystems Info;

        public List<Star> Stars;
        public List<Jumpgate> Jumpgates;
        public List<DB_wormholes> WormholeInfos;

        public Starsystem(DB_starsystems system)
        {
            Info = system;

            QueryAllStars();
            QueryAllJumpgates();
            QueryAllWormholes();
        }

        public void QueryAllStars()
        {
            Stars = new List<Star>();

            Debug.WriteLine(string.Format("StarSystem: {0}, Getting Stars", Info.id));
            var dbStars = Database.Session.Query<DB_stars>()
                .Where(x => x.starsystem_id == Info.id)
                .ToList();
            foreach (var star in dbStars)
                Stars.Add(new Star(star));
        }

        public void QueryAllJumpgates()
        {
            Jumpgates = new List<Jumpgate>();

            Debug.WriteLine(string.Format("StarSystem: {0}, Getting Jumpgates", Info.id));
            var dbJumpgates = Database.Session.Query<DB_jumpgates>()
                .Where(x => x.from_system_id == Info.id)
                .ToList();

            foreach (var dbJumpgate in dbJumpgates)
                Jumpgates.Add(new Jumpgate(dbJumpgate));
        }

        public void QueryAllWormholes()
        {
            WormholeInfos = new List<DB_wormholes>();

            Debug.WriteLine(string.Format("StarSystem: {0}, Getting Wormholes", Info.id));
            var dbWormholes = Database.Session.Query<DB_wormholes>()
                .Where (x => x.system_id_one == Info.id ||
                             x.system_id_two == Info.id)
                .ToList();

            foreach (var dbWormhole in dbWormholes)
                WormholeInfos.Add(dbWormhole);
        }
    }
}
