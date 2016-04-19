using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_battlegroups
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int starsystem_id { get; set; }

        public virtual string name { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_civilization_battlegroups : ClassMapping<DB_civilization_battlegroups>
    {
        public MAP_civilization_battlegroups()
        {
            Table("civilization_battlegroups");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.starsystem_id, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
