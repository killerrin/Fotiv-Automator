using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_experience_levels
    {
        public virtual int id { get; set; }
        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual int threshold { get; set; }

        public virtual int attack_bonus { get; set; }
        public virtual int special_attack_bonus { get; set; }
        public virtual int health_bonus { get; set; }
        public virtual int regeneration_bonus { get; set; }
        public virtual int agility_bonus { get; set; }
    }

    public class MAP_experience_levels : ClassMapping<DB_experience_levels>
    {
        public MAP_experience_levels()
        {
            Table("experience_levels");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.threshold, x => x.NotNullable(true));

            Property(x => x.attack_bonus, x => x.NotNullable(true));
            Property(x => x.special_attack_bonus, x => x.NotNullable(true));
            Property(x => x.health_bonus, x => x.NotNullable(true));
            Property(x => x.regeneration_bonus, x => x.NotNullable(true));
            Property(x => x.agility_bonus, x => x.NotNullable(true));
        }
    }
}
