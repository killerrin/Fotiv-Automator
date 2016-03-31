using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_species
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual int base_attack { get; set; }
        public virtual int base_special_attack { get; set; }
        public virtual int base_health { get; set; }
        public virtual int base_regeneration { get; set; }
        public virtual int base_agility { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_species : ClassMapping<DB_species>
    {
        public MAP_species()
        {
            Table("species");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));

            Property(x => x.base_attack, x => x.NotNullable(true));
            Property(x => x.base_special_attack, x => x.NotNullable(true));
            Property(x => x.base_health, x => x.NotNullable(true));
            Property(x => x.base_regeneration, x => x.NotNullable(true));
            Property(x => x.base_agility, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
