using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_rnd_units
    {
        public virtual int id { get; set; }
        public virtual int game_id { get; set; }

        public virtual int civilization_id { get; set; }
        public virtual int civ_struct_id { get; set; }
        public virtual int unit_id { get; set; }
        public virtual int? species_id { get; set; }

        public virtual string name { get; set; }
        public virtual int build_percentage { get; set; }
    }

    public class MAP_civilization_rnd_units : ClassMapping<DB_civilization_rnd_units>
    {
        public MAP_civilization_rnd_units()
        {
            Table("civ_rnd_units");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.civilization_id, x => {
                x.Column("civ_id");
                x.NotNullable(true);
            });

            Property(x => x.civ_struct_id, x => {
                x.Column("cstruct_id");
                x.NotNullable(true);
            });

            Property(x => x.unit_id, x => x.NotNullable(true));
            Property(x => x.species_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.build_percentage, x => x.NotNullable(true));
        }
    }
}
