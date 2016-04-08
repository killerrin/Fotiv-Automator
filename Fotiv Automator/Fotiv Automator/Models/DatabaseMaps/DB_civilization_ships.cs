using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_ships
    {
        public virtual int id { get; set; }

        public virtual int ship_id { get; set; }
        public virtual int? ship_battlegroup_id { get; set; }
        public virtual int starsystem_id { get; set; }
        public virtual int civilization_id { get; set; }

        public virtual int build_percentage { get; set; }
        public virtual int current_health { get; set; }
        public virtual bool command_and_control { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_civilization_ships : ClassMapping<DB_civilization_ships>
    {
        public MAP_civilization_ships()
        {
            Table("civilization_ships");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.ship_battlegroup_id, x => x.NotNullable(false));
            Property(x => x.ship_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.starsystem_id, x => x.NotNullable(true));

            Property(x => x.build_percentage, x => x.NotNullable(true));
            Property(x => x.current_health, x => x.NotNullable(true));
            Property(x => x.command_and_control, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
