using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_characters
    {
        public virtual int id { get; set; }

        public virtual int civilization_id { get; set; }
        public virtual int character_id { get; set; }
    }

    public class MAP_civilization_characters : ClassMapping<DB_civilization_characters>
    {
        public MAP_civilization_characters()
        {
            Table("civilization_characters");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.character_id, x => x.NotNullable(true));
        }
    }
}
