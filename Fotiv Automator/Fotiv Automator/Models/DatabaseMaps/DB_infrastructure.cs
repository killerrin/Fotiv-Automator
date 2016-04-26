using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_infrastructure
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }
        public virtual int rp_cost { get; set; }

        public virtual bool is_colony { get; set; }
        public virtual bool is_military { get; set; }

        public virtual int base_health { get; set; }
        public virtual int base_regeneration { get; set; }
        public virtual int base_attack { get; set; }
        public virtual int base_special_attack { get; set; }

        public virtual int influence_bonus { get; set; }

        public virtual int rp_bonus { get; set; }
        public virtual int science_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int unit_training_bonus { get; set; }

        public virtual int research_slots { get; set; }
        public virtual int ship_construction_slots { get; set; }
        public virtual int colonial_development_slots { get; set; }
        public virtual int unit_training_slots { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_infrastructure : ClassMapping<DB_infrastructure>
    {
        public MAP_infrastructure()
        {
            Table("infrastructure");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));
            Property(x => x.rp_cost, x => x.NotNullable(true));

            Property(x => x.is_colony, x => x.NotNullable(true));
            Property(x => x.is_military, x => x.NotNullable(true));

            Property(x => x.base_health, x => x.NotNullable(true));
            Property(x => x.base_regeneration, x => x.NotNullable(true));
            Property(x => x.base_attack, x => x.NotNullable(true));
            Property(x => x.base_special_attack, x => x.NotNullable(true));

            Property(x => x.influence_bonus, x => x.NotNullable(true));

            Property(x => x.rp_bonus, x => x.NotNullable(true));
            Property(x => x.science_bonus, x => x.NotNullable(true));
            Property(x => x.ship_construction_bonus, x => x.NotNullable(true));
            Property(x => x.colonial_development_bonus, x => x.NotNullable(true));
            Property(x => x.unit_training_bonus, x => x.NotNullable(true));

            Property(x => x.research_slots, x => x.NotNullable(true));
            Property(x => x.ship_construction_slots, x => x.NotNullable(true));
            Property(x => x.colonial_development_slots, x => x.NotNullable(true));
            Property(x => x.unit_training_slots, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
