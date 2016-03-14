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
        public DB_users UserInfo;
        public DB_user_civilizations UserCivilizationsInfo;

        public CivilizationOwner(DB_users user, DB_user_civilizations civUser)
        {
            UserInfo = user;
            UserCivilizationsInfo = civUser;
        }
    }
}
