using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }

        public virtual int? civilization_traits_1_id { get; set; }
        public virtual int? civilization_traits_2_id { get; set; }
        public virtual int? civilization_traits_3_id { get; set; }
        public virtual int? tech_level_id { get; set; }
        

        public virtual string name { get; set; }
        public virtual string colour { get; set; }

        public virtual int rp { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_civilization : ClassMapping<DB_civilization>
    {
        public MAP_civilization()
        {
            Table("civilization");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.civilization_traits_1_id, x => x.NotNullable(false));
            Property(x => x.civilization_traits_2_id, x => x.NotNullable(false));
            Property(x => x.civilization_traits_3_id, x => x.NotNullable(false));
            Property(x => x.tech_level_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.colour, x => x.NotNullable(true));

            Property(x => x.rp, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
