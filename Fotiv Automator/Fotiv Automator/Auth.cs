using NHibernate.Linq;
//using Fotiv_Automator.Models;
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

        //public static User User
        //{
        //    get
        //    {
        //        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        //            return null;
        //
        //        var user = HttpContext.Current.Items[UserKey] as User;
        //        if (user == null)
        //        {
        //            user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == HttpContext.Current.User.Identity.Name);
        //
        //            if (user == null)
        //                return null;
        //
        //            HttpContext.Current.Items[UserKey] = user;
        //        }
        //
        //        return user;
        //    }
        //}
    }
}
