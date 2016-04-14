using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_user_activity
    {
        public virtual int id { get; set; }

        public virtual int user_id { get; set; }
        public virtual DateTime last_active { get; set; }


        #region Helpers
        /// <summary>
        /// Returns a boolean value representing the Users Online Status
        /// </summary>
        public virtual bool IsOnline
        {
            get { return last_active > ActiveThreshold; }
        }

        /// <summary>
        /// Returns the datetime threshold for when a user is considered active
        /// </summary>
        public static DateTime ActiveThreshold
        {
            get { return DateTime.UtcNow.AddMinutes(-5); }
        }
        #endregion
    }

    public class MAP_user_activity : ClassMapping<DB_user_activity>
    {
        public MAP_user_activity()
        {
            Table("user_activity");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.user_id, x => x.NotNullable(true));
            Property(x => x.last_active, x => x.NotNullable(true));
        }
    }
}
