using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_research
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int research_id { get; set; }

        public virtual int build_percentage { get; set; }
    }

    public class MAP_civilization_research : ClassMapping<DB_civilization_research>
    {
        public MAP_civilization_research()
        {
            Table("civilization_research");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.research_id, x => x.NotNullable(true));

            Property(x => x.build_percentage, x => x.NotNullable(true));
        }
    }
}
