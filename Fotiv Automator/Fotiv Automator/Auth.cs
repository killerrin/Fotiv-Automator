using NHibernate.Linq;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;

namespace Fotiv_Automator
{
    public static class Auth
    {
        private const string UserKey = "Fotiv_Automator.Auth.UserKey";
        public static DB_users User
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;
        
                var user = HttpContext.Current.Items[UserKey] as DB_users;
                if (user == null)
                {
                    user = Database.Session.Query<DB_users>().FirstOrDefault(u => u.username == HttpContext.Current.User.Identity.Name);
        
                    if (user == null)
                        return null;
        
                    HttpContext.Current.Items[UserKey] = user;
                }
        
                return user;
            }
        }

        public static void UpdateUserActivity()
        {
            DB_users user = Auth.User;
            if (user == null)
                return;

            //Debug.WriteLine("Updating User Activity");
            var userActivity = Database.Session.Query<DB_user_activity>()
                .Where(x => x.user_id == user.id)
                .FirstOrDefault();

            if (userActivity == null)
            {
                //Debug.WriteLine("Updating User Activity: New Activity Entry");
                userActivity = new DB_user_activity { user_id = user.id };
            }

            userActivity.last_active = DateTime.UtcNow;

            Database.Session.SaveOrUpdate(userActivity);
            Database.Session.Flush();
        }
    }
}
