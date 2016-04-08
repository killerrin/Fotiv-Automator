using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_user_civilizations
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual int user_id { get; set; }
        public virtual int civilization_id { get; set; }

        public DB_user_civilizations() { }
        public DB_user_civilizations(int userID, int civilizationID, int? gameID)
        {
            game_id = gameID;

            user_id = userID;
            civilization_id = civilizationID;
        }
    }

    public class MAP_user_civilizations : ClassMapping<DB_user_civilizations>
    {
        public MAP_user_civilizations()
        {
            Table("user_civilizations");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.user_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
        }
    }
}
