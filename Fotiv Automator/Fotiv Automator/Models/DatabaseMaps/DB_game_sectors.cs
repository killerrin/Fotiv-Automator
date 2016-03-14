using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_game_sectors
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int sector_id { get; set; }
    }

    public class MAP_game_sectors : ClassMapping<DB_game_sectors>
    {
        public MAP_game_sectors()
        {
            Table("game_sectors");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.sector_id, x => x.NotNullable(true));
        }
    }
}
