﻿using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_species
    {
        public virtual int id { get; set; }

        public virtual int civilization_id { get; set; }
        public virtual int species_id { get; set; }
    }

    public class MAP_civilization_species : ClassMapping<DB_civilization_species>
    {
        public MAP_civilization_species()
        {
            Table("civilization_species");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.species_id, x => x.NotNullable(true));
        }
    }
}