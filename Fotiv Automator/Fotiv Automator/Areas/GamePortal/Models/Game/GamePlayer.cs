using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class GamePlayer
    {
        public int ID { get { return User.ID; } }
        public bool IsGM { get { return GameUserInfo.is_gm; } }

        public SafeUser User;
        public DB_game_users GameUserInfo;

        public GamePlayer() { }
        public GamePlayer(DB_users user, DB_game_users gameUser)
        {
            User = user;
            GameUserInfo = gameUser;
        }
    }
}
