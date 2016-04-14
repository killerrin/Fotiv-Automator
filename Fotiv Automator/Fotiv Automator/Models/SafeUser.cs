using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models
{
    public class SafeUser
    {
        public static bool IsCurrentUserAdmin { get { return System.Web.HttpContext.Current.User.IsInRole("Admin"); } }

        public int ID { get; protected set; }
        public string Username { get; protected set; }
        public bool IsOnline
        {
            get
            {
                var activity = GetUserActivity();
                if (activity == null)
                    return false;
                return activity.IsOnline;
            }
        }

        public SafeUser(int id, string username)
        {
            ID = id;
            Username = username;
        }


        public DB_user_activity GetUserActivity()
        {
            return Database.Session.Query<DB_user_activity>()
                    .Where(x => x.user_id == ID)
                    .FirstOrDefault();
        }

        public static implicit operator SafeUser(DB_users other)
        {
            return new SafeUser(other.id, other.username);
        }

        public static explicit operator DB_users(SafeUser other)
        {
            return Database.Session.Query<DB_users>().Where(x => x.id == other.ID).First();
        }
    }
}
