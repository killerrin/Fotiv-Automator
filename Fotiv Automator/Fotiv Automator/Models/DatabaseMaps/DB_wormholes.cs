using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_wormholes
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }

        public virtual int system_id_one { get; set; }
        public virtual int system_id_two { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_wormholes : ClassMapping<DB_wormholes>
    {
        public MAP_wormholes()
        {
            Table("wormholes");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.system_id_one, x => x.NotNullable(true));
            Property(x => x.system_id_two, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
