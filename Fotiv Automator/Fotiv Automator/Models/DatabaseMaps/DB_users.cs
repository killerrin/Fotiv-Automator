using Fotiv_Automator.Infrastructure.Extensions;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_users
    {
        private const int WorkFactor = 13;
        public static void FakeHash() { BCrypt.Net.BCrypt.HashPassword("", WorkFactor); }

        public virtual int id { get; set; }

        public virtual string username { get; set; }
        public virtual string email { get; set; }
        public virtual string password_hash { get; set; }

        public virtual DateTime? password_expiry { get; set; }

        #region Sets
        public virtual bool SetUsername(string _username)
        {
            if (!ValidateUsername(_username)) return false;

            username = _username;
            return true;
        }

        public virtual bool SetEmail(string _email)
        {
            if (!ValidateEmail(_email)) return false;

            email = _email;
            return true;
        }

        public virtual bool SetPassword(string _password)
        {
            //if (!ValidatePassword(_password)) return false;

            password_hash = BCrypt.Net.BCrypt.HashPassword(_password, WorkFactor);
            return true;
        }
        #endregion

        #region Validation
        public static bool ValidateUsername(string _username)
        {
            if (!IntExtensions.IsBetween(_username.Length, 1, 40))
                return false;

            if (!Regex.IsMatch(_username, @"^[a-zA-Z0-9\s]"))
                return false;

            return true;
        }

        public static bool ValidateEmail(string _email)
        {
            if (Regex.IsMatch(_email, @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[!#$%&'*+/=?`{|}~^-]+)*@(?:[A-Z0-9-]+\.)+[A-Z]{2,6}$"))
                return false;
            return true;
        }

        public static bool ValidatePassword(string _password)
        {
            // Check password contains at least one digit, one lower case 
            // letter, one uppercase letter, and is between 8 and 10 characters long
            if (!Regex.IsMatch(_password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$"))
                return false;
            return true;
        }

        public virtual bool CheckPassword(string password) { return BCrypt.Net.BCrypt.Verify(password, password_hash); }
        public virtual bool CheckPasswordExpiry()
        {
            if (password_expiry == null) return false;
            return password_expiry.Value <= DateTime.UtcNow; 
        }
        #endregion
    }

    public class MAP_users : ClassMapping<DB_users>
    {
        public MAP_users()
        {
            Table("users");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.username, x => x.NotNullable(true));
            Property(x => x.email, x => x.NotNullable(true));
            Property(x => x.password_hash, x => x.NotNullable(true));

            Property(x => x.password_expiry, x => x.NotNullable(false));
        }
    }
}
