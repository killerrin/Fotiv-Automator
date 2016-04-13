using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_star_ages
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
    }

    public class MAP_star_ages : ClassMapping<DB_star_ages>
    {
        public MAP_star_ages()
        {
            Table("star_ages");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
        }
    }
}
