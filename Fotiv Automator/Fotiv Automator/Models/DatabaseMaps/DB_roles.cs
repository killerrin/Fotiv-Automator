using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_roles
    {
        public virtual int id { get; set; }

        public virtual string name { get; set; }
    }

    public class MAP_roles : ClassMapping<DB_roles>
    {
        public MAP_roles()
        {
            Table("roles");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.name, x => x.NotNullable(true));
        }
    }
}
