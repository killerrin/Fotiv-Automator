using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.ViewModels;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fotiv_Automator.Controllers
{
    public class UserAccountController : Controller
    {
        public ActionResult Index()
        {
            Debug.WriteLine(string.Format("GET: User Account Controller: Index"));
            if (!User.Identity.IsAuthenticated)
                return RedirectToRoute("login");

            var user = Auth.User;
            if (user == null)
                return HttpNotFound();

            return View(new UserAccountUpdateForm
            {
                ID = user.id,
                Username = user.username,
                Email = user.email
            });
        }

        [HttpPost, ValidateAntiForgeryToken, HttpParamAction]
        public ActionResult Save(UserAccountUpdateForm form)
        {
            var user = Auth.User;
            if (user == null)
                return HttpNotFound();

            if (!string.IsNullOrWhiteSpace(form.Username) || !string.IsNullOrWhiteSpace(form.Email) || !string.IsNullOrWhiteSpace(form.NewPassword) || !string.IsNullOrWhiteSpace(form.VerifyNewPassword))
            {
                if (user.CheckPassword(form.CurrentPassword))
                {
                    #region New Username
                    if (!user.username.Equals(form.Username))
                    {
                        if (DB_users.ValidateUsername(form.Username))
                        {
                            if (Database.Session.Query<DB_users>().FirstOrDefault(u => u.username == form.Username) != null)
                                ModelState.AddModelError("Username", "Username has already been taken");
                            else
                            {
                                user.SetUsername(form.Username);

                                // Reset the Auth Cookie if the user changes their username
                                FormsAuthentication.SignOut();
                                FormsAuthentication.SetAuthCookie(user.username, true);
                            }
                        }
                        else
                            ModelState.AddModelError("Username", "Username is invalid");
                    }
                    #endregion

                    #region New Email
                    if (!user.email.Equals(form.Email))
                    {
                        if (DB_users.ValidateEmail(form.Email))
                        {
                            //if (Database.Session.Query<DB_users>().FirstOrDefault(u => u.email == form.Email) != null)
                            //    ModelState.AddModelError("Email", "Email has already been taken");
                            user.SetEmail(form.Email);
                        }
                        else
                            ModelState.AddModelError("Email", "Email is invalid");
                    }
                    #endregion

                    #region New Password
                    if (!string.IsNullOrWhiteSpace(form.NewPassword) && !string.IsNullOrWhiteSpace(form.VerifyNewPassword))
                    {
                        if (form.NewPassword.Equals(form.VerifyNewPassword))
                        {
                            user.SetPassword(form.NewPassword);
                        }
                        else
                        {
                            ModelState.AddModelError("NewPassword", "Check that your new password was written the same way both times");
                        }
                    }
                    #endregion
                }
                else
                    ModelState.AddModelError("Password", "You have entered your current password incorrectly");
            }



            // Update the ID, Save the Object and then return
            form.ID = user.id;
            Database.Session.Update(user);

            if (!ModelState.IsValid)
                return View(form);

            return RedirectToRoute("home");
        }
    }
}