using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class CivilizationOwner
    {
        public int ID { get { return User.ID; } }

        public SafeUser User;
        public DB_user_civilizations UserCivilizationsInfo;

        public CivilizationOwner(DB_users user, DB_user_civilizations civUser)
        {
            User = user;
            UserCivilizationsInfo = civUser;
        }
    }
}
