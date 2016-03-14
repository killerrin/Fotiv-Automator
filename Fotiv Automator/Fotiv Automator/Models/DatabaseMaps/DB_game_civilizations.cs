using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_game_civilizations
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
    }

    public class MAP_game_civilizations : ClassMapping<DB_game_civilizations>
    {
        public MAP_game_civilizations()
        {
            Table("game_civilizations");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
        }
    }
}
