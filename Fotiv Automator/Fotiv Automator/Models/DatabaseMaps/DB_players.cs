using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_players
    {
        public virtual int id { get; set; }
        public virtual string username { get; set; }
        public virtual string email { get; set; }
        public virtual string password_hash { get; set; }
    }
}
