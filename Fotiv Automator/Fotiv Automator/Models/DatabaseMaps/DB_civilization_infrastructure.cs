using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_infrastructure
    {
        public virtual int id { get; set; }

        public virtual int civilization_id { get; set; }
        public virtual int planet_id { get; set; }
        public virtual int struct_id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual int build_percentage { get; set; }
        public virtual int current_health { get; set; }

        public virtual bool can_upgrade { get; set; }
        public virtual bool is_military { get; set; }

        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_civilization_infrastructure : ClassMapping<DB_civilization_infrastructure>
    {
        public MAP_civilization_infrastructure()
        {
            Table("civilization_infrastructure");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.planet_id, x => x.NotNullable(true));
            Property(x => x.struct_id, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));

            Property(x => x.build_percentage, x => x.NotNullable(true));
            Property(x => x.current_health, x => x.NotNullable(true));
            Property(x => x.can_upgrade, x => x.NotNullable(true));
            Property(x => x.is_military, x => x.NotNullable(true));
            Property(x => x.notes, x => x.NotNullable(false));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
