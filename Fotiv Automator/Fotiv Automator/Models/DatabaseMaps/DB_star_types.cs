using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_star_types
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
    }

    public class MAP_star_types : ClassMapping<DB_star_types>
    {
        public MAP_star_types()
        {
            Table("star_types");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
        }
    }
}
