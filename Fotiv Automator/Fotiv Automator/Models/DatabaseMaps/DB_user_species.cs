using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_user_species
    {
        public virtual int id { get; set; }

        public virtual int user_id { get; set; }
        public virtual int species_id { get; set; }
    }

    public class MAP_user_species : ClassMapping<DB_user_species>
    {
        public MAP_user_species()
        {
            Table("user_species");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.user_id, x => x.NotNullable(true));
            Property(x => x.species_id, x => x.NotNullable(true));
        }
    }
}
