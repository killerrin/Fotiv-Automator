using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_starsystems
    {
        public virtual int id { get; set; }

        public virtual int sector_id { get; set; }

        public virtual int hex_x { get; set; }
        public virtual int hex_y { get; set; }
    }

    public class MAP_starsystems : ClassMapping<DB_starsystems>
    {
        public MAP_starsystems()
        {
            Table("starsystems");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.sector_id, x => x.NotNullable(true));

            Property(x => x.hex_x, x => x.NotNullable(true));
            Property(x => x.hex_y, x => x.NotNullable(true));
        }
    }
}
