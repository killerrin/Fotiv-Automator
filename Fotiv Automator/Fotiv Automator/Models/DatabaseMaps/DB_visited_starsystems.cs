
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_visited_starsystems
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id { get; set; }
        public virtual int starsystem_id { get; set; }

        public DB_visited_starsystems() { }
        public DB_visited_starsystems(int starsystemID, int civilizationID, int gameID)
        {
            game_id = gameID;

            starsystem_id = starsystemID;
            civilization_id = civilizationID;
        }
    }

    public class MAP_visited_starsystems : ClassMapping<DB_visited_starsystems>
    {
        public MAP_visited_starsystems()
        {
            Table("visited_starsystems");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.starsystem_id, x => x.NotNullable(true));
        }
    }
}
