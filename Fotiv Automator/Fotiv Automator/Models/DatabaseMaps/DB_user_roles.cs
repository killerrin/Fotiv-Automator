using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_user_roles
    {
        public virtual int id { get; set; }

        public virtual int user_id { get; set; }
        public virtual int role_id { get; set; }
    }

    public class MAP_user_roles : ClassMapping<DB_user_roles>
    {
        public MAP_user_roles()
        {
            Table("user_roles");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.user_id, x => x.NotNullable(true));
            Property(x => x.role_id, x => x.NotNullable(true));
        }

        public static List<DB_user_roles> QueryManually()
        {
            var userIDs = Database.Session.CreateSQLQuery("SELECT user_id FROM user_roles;").List<int>();
            var roleIDs = Database.Session.CreateSQLQuery("SELECT role_id FROM user_roles;").List<int>();

            List<DB_user_roles> query = new System.Collections.Generic.List<DB_user_roles>();
            for (int i = 0; i < userIDs.Count; i++)
            {
                query.Add(new DB_user_roles
                {
                    user_id = userIDs[i],
                    role_id = roleIDs[i],
                });
            }
            return query;
        }
    }
}
