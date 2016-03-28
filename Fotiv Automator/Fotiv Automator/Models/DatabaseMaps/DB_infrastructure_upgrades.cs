using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_infrastructure_upgrades
    {
        public virtual int id { get; set; }

        public virtual int from_infra_id { get; set; }
        public virtual int to_infra_id { get; set; }

        public DB_infrastructure_upgrades() { }
        public DB_infrastructure_upgrades(int fromID, int toID)
        {
            from_infra_id = fromID;
            to_infra_id = toID;
        }
    }

    public class MAP_infrastructure_upgrades : ClassMapping<DB_infrastructure_upgrades>
    {
        public MAP_infrastructure_upgrades()
        {
            Table("infrastructure_upgrades");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.from_infra_id, x => x.NotNullable(true));
            Property(x => x.to_infra_id, x => x.NotNullable(true));
        }
    }
}
