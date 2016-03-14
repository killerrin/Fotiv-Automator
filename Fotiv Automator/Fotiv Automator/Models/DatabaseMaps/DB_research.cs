using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_research
    {
        public virtual int id { get; set; }

        public virtual int rp_cost { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual int attack_bonus { get; set; }
        public virtual int health_bonus { get; set; }

        public virtual int science_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_research : ClassMapping<DB_research>
    {
        public MAP_research()
        {
            Table("research");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.rp_cost, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));

            Property(x => x.attack_bonus, x => x.NotNullable(true));
            Property(x => x.health_bonus, x => x.NotNullable(true));

            Property(x => x.science_bonus, x => x.NotNullable(true));
            Property(x => x.colonial_development_bonus, x => x.NotNullable(true));
            Property(x => x.ship_construction_bonus, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
