using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_met
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int civilization_id1 { get; set; }
        public virtual int civilization_id2 { get; set; }

        public DB_civilization_met() { }
        public DB_civilization_met(int civilizationID1, int civilizationID2, int gameID)
        {
            game_id = gameID;

            civilization_id1 = civilizationID1;
            civilization_id2 = civilizationID2;
        }
    }

    public class MAP_civilization_met : ClassMapping<DB_civilization_met>
    {
        public MAP_civilization_met()
        {
            Table("civilization_met");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id1, x => x.NotNullable(true));
            Property(x => x.civilization_id2, x => x.NotNullable(true));
        }
    }
}
