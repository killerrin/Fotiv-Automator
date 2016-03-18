using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Game
{
    public class User
    {
        public int ID { get; protected set; }
        public string Username { get; protected set; }

        public User(int id, string username)
        {
            ID = id;
            Username = username;
        }

        public static implicit operator User(DB_users other)
        {
            return new User(other.id, other.username);
        }

        public static explicit operator DB_users(User other)
        {
            return Database.Session.Query<DB_users>().Where(x => x.id == other.ID).First();
        }
    }
}
