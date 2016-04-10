using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_planets
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int star_id { get; set; }

        public virtual int? orbiting_planet_id { get; set; }
        public virtual int? planet_tier_id { get; set; }

        public virtual string name { get; set; }
        public virtual string stage_of_life { get; set; }
        public virtual int resources { get; set; }
        public virtual bool supports_colonies { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_planets : ClassMapping<DB_planets>
    {
        public MAP_planets()
        {
            Table("planets");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.star_id, x => x.NotNullable(true));

            Property(x => x.orbiting_planet_id, x => x.NotNullable(false));
            Property(x => x.planet_tier_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.stage_of_life, x => x.NotNullable(true));
            Property(x => x.resources, x => x.NotNullable(true));
            Property(x => x.supports_colonies, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
