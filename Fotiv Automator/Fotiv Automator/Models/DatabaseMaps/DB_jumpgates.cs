using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_jumpgates
    {
        public virtual int id { get; set; }

        public virtual int? game_id { get; set; }

        public virtual int civ_struct_id { get; set; }
        public virtual int from_system_id { get; set; }
        public virtual int to_system_id { get; set; }

        public virtual string gmnotes { get; set; }
    }

    public class MAP_jumpgates : ClassMapping<DB_jumpgates>
    {
        public MAP_jumpgates()
        {
            Table("jumpgates");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(false));

            Property(x => x.civ_struct_id, x => x.NotNullable(true));
            Property(x => x.from_system_id, x => x.NotNullable(true));
            Property(x => x.to_system_id, x => x.NotNullable(true));

            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
