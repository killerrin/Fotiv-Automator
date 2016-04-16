using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_units
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }
        public virtual int rp_cost { get; set; }
        public virtual int num_build { get; set; }
        public virtual int build_rate { get; set; }
        public virtual bool is_military { get; set; }

        public virtual int base_attack { get; set; }
        public virtual int base_special_attack { get; set; }
        public virtual int base_health { get; set; }
        public virtual int base_regeneration { get; set; }
        public virtual int base_agility { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_units : ClassMapping<DB_units>
    {
        public MAP_units()
        {
            Table("units");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));
            Property(x => x.rp_cost, x => x.NotNullable(true));
            Property(x => x.num_build, x => x.NotNullable(true));
            Property(x => x.build_rate, x => x.NotNullable(true));
            Property(x => x.is_military, x => x.NotNullable(true));

            Property(x => x.base_attack, x => x.NotNullable(true));
            Property(x => x.base_special_attack, x => x.NotNullable(true));
            Property(x => x.base_health, x => x.NotNullable(true));
            Property(x => x.base_regeneration, x => x.NotNullable(true));
            Property(x => x.base_agility, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
