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

        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }
        public virtual int rp_cost { get; set; }

        public virtual bool apply_military { get; set; }
        public virtual bool apply_units { get; set; }
        public virtual bool apply_ships { get; set; }
        public virtual bool apply_infrastructure { get; set; }

        public virtual int domestic_influence_bonus { get; set; }
        public virtual int foreign_influence_bonus { get; set; }

        public virtual int attack_bonus { get; set; }
        public virtual int special_attack_bonus { get; set; }
        public virtual int health_bonus { get; set; }
        public virtual int regeneration_bonus { get; set; }
        public virtual int agility_bonus { get; set; }

        public virtual int science_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }
        public virtual int unit_training_bonus { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_research : ClassMapping<DB_research>
    {
        public MAP_research()
        {
            Table("research");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));
            Property(x => x.rp_cost, x => x.NotNullable(true));

            Property(x => x.apply_military, x => x.NotNullable(true));
            Property(x => x.apply_units, x => x.NotNullable(true));
            Property(x => x.apply_ships, x => x.NotNullable(true));
            Property(x => x.apply_infrastructure, x => x.NotNullable(true));

            Property(x => x.domestic_influence_bonus, x => x.NotNullable(true));
            Property(x => x.foreign_influence_bonus, x => x.NotNullable(true));

            Property(x => x.attack_bonus, x => x.NotNullable(true));
            Property(x => x.special_attack_bonus, x => x.NotNullable(true));
            Property(x => x.health_bonus, x => x.NotNullable(true));
            Property(x => x.regeneration_bonus, x => x.NotNullable(true));
            Property(x => x.agility_bonus, x => x.NotNullable(true));

            Property(x => x.science_bonus, x => x.NotNullable(true));
            Property(x => x.colonial_development_bonus, x => x.NotNullable(true));
            Property(x => x.ship_construction_bonus, x => x.NotNullable(true));
            Property(x => x.unit_training_bonus, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
