using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class CivilizationOwner
    {
        public User User;
        public DB_user_civilizations UserCivilizationsInfo;

        public CivilizationOwner(DB_users user, DB_user_civilizations civUser)
        {
            User = user;
            UserCivilizationsInfo = civUser;
            
            //UserInfo.email = "";
            //UserInfo.password_hash = "";
            //UserInfo.password_expiry = null;
        }
    }
}
