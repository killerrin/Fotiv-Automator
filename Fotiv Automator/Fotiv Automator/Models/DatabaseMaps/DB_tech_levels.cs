using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_tech_levels
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual int attack_detriment { get; set; }
    }

    public class MAP_tech_levels : ClassMapping<DB_tech_levels>
    {
        public MAP_tech_levels()
        {
            Table("tech_levels");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.attack_detriment, x => x.NotNullable(true));
        }
    }
}
