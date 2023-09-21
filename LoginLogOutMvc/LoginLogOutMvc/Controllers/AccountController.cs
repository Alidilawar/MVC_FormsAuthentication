using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginLogOutMvc.Models;
using System.Web.Security;

namespace LoginLogOutMvc.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MemberShip model)
        {
            using (var context = new OfficeEntities())
            {
                string key = "drfjkjdvkvdvvfbkbv12154gffgfsdvv";
                int n = key.Length;
                var UserNameEncrypt = Utilities.Encrypt(key, model.UserName);
                var PsdEncrypt = Utilities.Encrypt(key, model.Password);

                var UsrName = Utilities.Decrypt(key, UserNameEncrypt);
                var Password = Utilities.Decrypt(key, PsdEncrypt);

                bool isvalid = context.Users.Any(x => x.UserName == UsrName && x.Password == Password);
                if (isvalid)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Employees");
                }
                ModelState.AddModelError("", "Invalid UserName and Password...");
            }
            return View();
        }

        // GET: Account
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            using (var context = new OfficeEntities())
            {
                context.Users.Add(model);
                context.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}