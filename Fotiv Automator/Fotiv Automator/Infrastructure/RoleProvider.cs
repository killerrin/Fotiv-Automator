using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fotiv_Automator.Infrastructure
{
	public class RoleProvider : System.Web.Security.RoleProvider
	{
		public override string[] GetRolesForUser(string username)
		{
			var loggedInUser = Auth.User;
			if (loggedInUser == null) return new string[] { };

			var dbRoles = Database.Session.Query<DB_roles>().ToList();
			var dbUserRoles = Database.Session.Query<DB_user_roles>()
				.Where(x => x.user_id == loggedInUser.id)
				.ToList();

			List<string> userRoles = new List<string>();
			foreach (var dbRole in dbRoles)
				foreach (var dbUserRole in dbUserRoles)
					if (dbRole.id == dbUserRole.role_id)
						userRoles.Add(dbRole.name);

			return userRoles.ToArray();
		}

		#region Not Implemented
		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}