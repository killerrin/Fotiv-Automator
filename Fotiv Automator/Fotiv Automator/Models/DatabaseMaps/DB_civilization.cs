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
        
        public virtual string name { get; set; }
        public virtual string colour { get; set; }
        public virtual string website { get; set; }

        public virtual int rp { get; set; }

        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_civilization : ClassMapping<DB_civilization>
    {
        public MAP_civilization()
        {
            Table("civilization");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.colour, x => x.NotNullable(true));
            Property(x => x.website, x => x.NotNullable(false));

            Property(x => x.rp, x => x.NotNullable(true));

            Property(x => x.notes, x => x.NotNullable(false));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
