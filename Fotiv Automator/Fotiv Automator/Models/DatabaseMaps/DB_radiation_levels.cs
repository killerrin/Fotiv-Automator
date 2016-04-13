using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_radiation_levels
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
    }

    public class MAP_radiation_levels : ClassMapping<DB_radiation_levels>
    {
        public MAP_radiation_levels()
        {
            Table("radiation_levels");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
        }
    }
}
