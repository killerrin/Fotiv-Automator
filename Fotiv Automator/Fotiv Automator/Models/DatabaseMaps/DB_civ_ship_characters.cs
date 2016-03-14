using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civ_ship_characters
    {
        public virtual int id { get; set; }

        public virtual int civ_ship_id { get; set; }
        public virtual int character_id { get; set; }
    }

    public class MAP_civ_ship_characters : ClassMapping<DB_civ_ship_characters>
    {
        public MAP_civ_ship_characters()
        {
            Table("civ_ship_characters");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.civ_ship_id, x => x.NotNullable(true));
            Property(x => x.character_id, x => x.NotNullable(true));
        }
    }
}
