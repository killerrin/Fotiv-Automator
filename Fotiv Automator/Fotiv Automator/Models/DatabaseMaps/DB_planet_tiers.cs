using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_planet_tiers
    {
        public virtual int id { get; set; }

        public virtual string name { get; set; }
        public virtual int build_rate { get; set; }
    }

    public class MAP_planet_tiers : ClassMapping<DB_planet_tiers>
    {
        public MAP_planet_tiers()
        {
            Table("planet_tiers");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.build_rate, x => x.NotNullable(true));
        }
    }
}
