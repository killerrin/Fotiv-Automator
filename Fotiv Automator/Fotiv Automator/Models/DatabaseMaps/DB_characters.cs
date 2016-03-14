using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_characters
    {
        public virtual int id { get; set; }

        public virtual int species_id { get; set; }
        public virtual int starsystem_id { get; set; }

        public virtual string name { get; set; }
        public virtual string job { get; set; }
        public virtual string status { get; set; }
        public virtual string website { get; set; }

        public virtual int health { get; set; }
        public virtual int attack { get; set; }
        public virtual int influence { get; set; }

        public virtual int admiral_bonus { get; set; }
        public virtual int science_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }

        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_characters : ClassMapping<DB_characters>
    {
        public MAP_characters()
        {
            Table("characters");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.species_id, x => x.NotNullable(true));
            Property(x => x.starsystem_id, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.job, x => x.NotNullable(false));
            Property(x => x.status, x => x.NotNullable(false));
            Property(x => x.website, x => x.NotNullable(false));

            Property(x => x.health, x => x.NotNullable(true));
            Property(x => x.attack, x => x.NotNullable(true));
            Property(x => x.influence, x => x.NotNullable(true));

            Property(x => x.admiral_bonus, x => x.NotNullable(true));
            Property(x => x.science_bonus, x => x.NotNullable(true));
            Property(x => x.colonial_development_bonus, x => x.NotNullable(true));
            Property(x => x.ship_construction_bonus, x => x.NotNullable(true));

            Property(x => x.notes, x => x.NotNullable(false));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
