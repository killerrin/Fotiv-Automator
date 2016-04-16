using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_unit_battlegroups
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int planet_id { get; set; }

        public virtual string name { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_unit_battlegroups : ClassMapping<DB_unit_battlegroups>
    {
        public MAP_unit_battlegroups()
        {
            Table("unit_battlegroups");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.planet_id, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
