using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_games
    {
        public virtual int id { get; set; }

        public virtual string name { get; set; }
        public virtual string description { get; set; }

        public virtual bool opened_to_public { get; set; }
    }

    public class MAP_games : ClassMapping<DB_games>
    {
        public MAP_games()
        {
            Table("games");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.description, x => x.NotNullable(false));

            Property(x => x.opened_to_public, x => x.NotNullable(true));
        }
    }
}
