using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_ship_rates
    {
        public virtual int id { get; set; }

        public virtual string name { get; set; }
        public virtual int build_rate { get; set; }

        public override string ToString()
        {
            return $"{id}, {name}";
        }
    }

    public class MAP_ship_rates : ClassMapping<DB_ship_rates>
    {
        public MAP_ship_rates()
        {
            Table("ship_rates");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.build_rate, x => x.NotNullable(false));
        }
    }
}
