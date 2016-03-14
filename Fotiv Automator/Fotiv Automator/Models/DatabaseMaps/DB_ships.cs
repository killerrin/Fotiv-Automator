using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_ships
    {
        public virtual int id { get; set; }

        public virtual int? ship_rate_id { get; set; }

        public virtual int rp_cost { get; set; }
        public virtual int base_health { get; set; }
        public virtual int base_attack { get; set; }
        public virtual int maximum_fighters { get; set; }
        public virtual int num_build { get; set; }
        public virtual int gmnotes { get; set; }
    }

    public class MAP_ships : ClassMapping<DB_ships>
    {
        public MAP_ships()
        {
            Table("ships");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.ship_rate_id, x => x.NotNullable(false));

            Property(x => x.rp_cost, x => x.NotNullable(true));
            Property(x => x.base_health, x => x.NotNullable(true));
            Property(x => x.base_attack, x => x.NotNullable(true));
            Property(x => x.maximum_fighters, x => x.NotNullable(true));
            Property(x => x.num_build, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
