using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_traits
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual int local_influence_bonus { get; set; }
        public virtual int foreign_influence_bonus { get; set; }
        public virtual int trade_bonus { get; set; }

        public virtual bool apply_military { get; set; }
        public virtual bool apply_units { get; set; }
        public virtual bool apply_ships { get; set; }
        public virtual bool apply_infrastructure { get; set; }

        public virtual int science_bonus { get; set; }
        public virtual int colonial_development_bonus { get; set; }
        public virtual int ship_construction_bonus { get; set; }
        public virtual int unit_training_bonus { get; set; }
    }

    public class MAP_civilization_traits : ClassMapping<DB_civilization_traits>
    {
        public MAP_civilization_traits()
        {
            Table("civilization_traits");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));

            Property(x => x.local_influence_bonus, x => x.NotNullable(true));
            Property(x => x.foreign_influence_bonus, x => x.NotNullable(true));
            Property(x => x.trade_bonus, x => x.NotNullable(true));

            Property(x => x.apply_military, x => x.NotNullable(true));
            Property(x => x.apply_units, x => x.NotNullable(true));
            Property(x => x.apply_ships, x => x.NotNullable(true));
            Property(x => x.apply_infrastructure, x => x.NotNullable(true));

            Property(x => x.science_bonus, x => x.NotNullable(true));
            Property(x => x.colonial_development_bonus, x => x.NotNullable(true));
            Property(x => x.ship_construction_bonus, x => x.NotNullable(true));
            Property(x => x.unit_training_bonus, x => x.NotNullable(true));
        }
    }
}
