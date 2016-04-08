using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_game_users
    {
        public virtual int id { get; set; }

        public virtual int user_id { get; set; }
        public virtual int game_id { get; set; }

        public virtual bool is_gm { get; set; }
    }

    public class MAP_game_users : ClassMapping<DB_game_users>
    {
        public MAP_game_users()
        {
            Table("game_users");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.user_id, x => x.NotNullable(true));
            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.is_gm, x => x.NotNullable(true));
        }
    }
}
