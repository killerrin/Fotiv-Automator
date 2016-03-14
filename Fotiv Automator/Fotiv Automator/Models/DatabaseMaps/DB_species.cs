using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_species
    {
        public virtual int id { get; set; }

        public virtual string name { get; set; }
        public virtual string website { get; set; }
        public virtual string notes { get; set; }
        public virtual string gmnotes { get; set; }
    }

    public class MAP_species : ClassMapping<DB_species>
    {
        public MAP_species()
        {
            Table("species");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.website, x => x.NotNullable(false));
            Property(x => x.notes, x => x.NotNullable(false));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
