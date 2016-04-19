using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_unit_categories
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual int build_rate { get; set; }
        public virtual bool is_military { get; set; }

        public override string ToString()
        {
            return $"{id}, {name}";
        }
    }

    public class MAP_unit_categories : ClassMapping<DB_unit_categories>
    {
        public MAP_unit_categories()
        {
            Table("unit_categories");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.build_rate, x => x.NotNullable(true));
            Property(x => x.is_military, x => x.NotNullable(true));
        }
    }
}
