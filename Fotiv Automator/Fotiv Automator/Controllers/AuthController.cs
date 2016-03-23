using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fotiv_Automator.ViewModels;
using NHibernate.Linq;
using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using System.Diagnostics;

namespace Fotiv_Automator.Controllers
{
	public class AuthController : Controller
	{
		#region Login     
		[AllowAnonymous]
		[HttpGet]
		public ActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToRoute("home");

			Debug.WriteLine(string.Format("GET: Auth Controller: Login"));
			return View(new AuthLoginForm());
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(AuthLoginForm form, string returnUrl)
		{
			Debug.WriteLine(string.Format("POST: Auth Controller: Login"));

			#region Check if Inputs are Valid
			if (!DB_users.ValidateUsername(form.Username))
				ModelState.AddModelError("Username", "Username contains invalid characters");

			if (!ModelState.IsValid)
				return View(form);
			#endregion

			var user = Database.Session.Query<DB_users>().FirstOrDefault(u => u.username == form.Username);
			
			// Prevent Timing Attacks
			if (user == null)
				DB_users.FakeHash();
			
			// Check Password and add Model error if incorrect
			if (user == null || !user.CheckPassword(form.Password))
				ModelState.AddModelError("Username", "Username or Password is incorrect");
			
			if (!ModelState.IsValid)
				return View(form);
			
			FormsAuthentication.SetAuthCookie(user.username, true);

			if (!string.IsNullOrWhiteSpace(returnUrl))
				return Redirect(returnUrl);

			return RedirectToRoute("home");
		}
		#endregion

		#region Logout
		[HttpGet, AllowAnonymous]
		public ActionResult Logout()
		{
			Debug.WriteLine(string.Format("GET: Auth Controller: Logout"));
			FormsAuthentication.SignOut();
			return RedirectToRoute("home");
		}
		#endregion

		#region Create Account
		[HttpGet, AllowAnonymous]
		public ActionResult CreateAccount()
		{
			Debug.WriteLine(string.Format("GET: Auth Controller: Create Account"));
			return View("CreateAccount", new AuthCreateAccountForm());
		}

		[HttpPost, AllowAnonymous]
		public ActionResult CreateAccount(AuthCreateAccountForm form, string returnUrl)
		{
			Debug.WriteLine(string.Format("POST: Auth Controller: Create Account"));

			#region Check if Inputs are Valid
			if (!DB_users.ValidateUsername(form.Username))
				ModelState.AddModelError("Username", "Username is invalid");

			if (!DB_users.ValidateEmail(form.Email))
				ModelState.AddModelError("Email", "Email is invalid");

			//if (!DB_users.ValidatePassword(form.Password))
			//	ModelState.AddModelError("Password", "Password is invalid");

			if (!ModelState.IsValid)
				return View(form);
			#endregion

			#region Check if Inputs have been Taken
			if (Database.Session.Query<DB_users>().FirstOrDefault(u => u.username == form.Username) != null)
				ModelState.AddModelError("Username", "Username has already been taken");

			//if (Database.Session.Query<DB_users>().FirstOrDefault(u => u.email == form.Email) != null)
			//    ModelState.AddModelError("Email", "Email has already been taken");

			if (!ModelState.IsValid)
				return View(form);
			#endregion

			#region Create the Account and Login
			DB_users newUser = new DB_users();
			newUser.SetUsername(form.Username);
			newUser.SetEmail(form.Email);
			newUser.SetPassword(form.Password);
			Database.Session.Save(newUser);

			FormsAuthentication.SetAuthCookie(newUser.username, true);
			#endregion

			if (!string.IsNullOrWhiteSpace(returnUrl))
				return Redirect(returnUrl);

			return RedirectToRoute("home");
		}
		#endregion
	}
}