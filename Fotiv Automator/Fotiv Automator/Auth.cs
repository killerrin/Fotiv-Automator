using NHibernate.Linq;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fotiv_Automator
{
    public class Auth
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
    }
}
