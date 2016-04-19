using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_units
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual int? unit_category_id { get; set; }

        public virtual string name { get; set; }
        public virtual string unit_type { get; set; }
        public virtual string description { get; set; }
        public virtual int rp_cost { get; set; }
        public virtual int number_to_build { get; set; }

        public virtual bool is_space_unit { get; set; }
        public virtual bool can_embark { get; set; }

        public virtual bool can_attack_ground_units { get; set; }
        public virtual bool can_attack_boats { get; set; }
        public virtual bool can_attack_planes { get; set; }
        public virtual bool can_attack_spaceships { get; set; }

        public virtual int embarking_slots { get; set; }
        public virtual int negate_damage { get; set; }

        public virtual int base_attack { get; set; }
        public virtual int base_special_attack { get; set; }
        public virtual int base_health { get; set; }
        public virtual int base_regeneration { get; set; }
        public virtual int base_agility { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_units : ClassMapping<DB_units>
    {
        public MAP_units()
        {
            Table("units");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.unit_category_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.unit_type, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));
            Property(x => x.rp_cost, x => x.NotNullable(true));
            Property(x => x.number_to_build, x => x.NotNullable(true));

            Property(x => x.is_space_unit, x => x.NotNullable(true));
            Property(x => x.can_embark, x => x.NotNullable(true));

            Property(x => x.can_attack_ground_units, x => x.NotNullable(true));
            Property(x => x.can_attack_boats, x => x.NotNullable(true));
            Property(x => x.can_attack_planes, x => x.NotNullable(true));
            Property(x => x.can_attack_spaceships, x => x.NotNullable(true));

            Property(x => x.embarking_slots, x => x.NotNullable(true));
            Property(x => x.negate_damage, x => x.NotNullable(true));

            Property(x => x.base_attack, x => x.NotNullable(true));
            Property(x => x.base_special_attack, x => x.NotNullable(true));
            Property(x => x.base_health, x => x.NotNullable(true));
            Property(x => x.base_regeneration, x => x.NotNullable(true));
            Property(x => x.base_agility, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
