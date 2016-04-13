using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_stars
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int starsystem_id { get; set; }

        public virtual int? star_type_id { get; set; }
        public virtual int? star_age_id { get; set; }
        public virtual int? radiation_level_id { get; set; }

        public virtual string name { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_stars : ClassMapping<DB_stars>
    {
        public MAP_stars()
        {
            Table("stars");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.starsystem_id, x => x.NotNullable(true));

            Property(x => x.star_type_id, x => x.NotNullable(false));
            Property(x => x.star_age_id, x => x.NotNullable(false));
            Property(x => x.radiation_level_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
