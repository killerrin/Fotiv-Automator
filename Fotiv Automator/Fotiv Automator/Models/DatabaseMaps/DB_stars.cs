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

        public virtual string name { get; set; }
        public virtual string age { get; set; }
        public virtual string radiation_level { get; set; }
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

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.age, x => x.NotNullable(true));
            Property(x => x.radiation_level, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
