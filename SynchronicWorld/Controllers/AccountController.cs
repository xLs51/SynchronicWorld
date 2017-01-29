using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class AccountController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Account/
        [HttpPost]
        public ActionResult Index(SignInModel signIn)
        {
            if (!ModelState.IsValid) return View();
            // Hash password
            var password = CreatePasswordHash(signIn.Password);
            // If user found in the db
            var user = db.Users.FirstOrDefault(u => u.Username == signIn.Username && u.Password == password);

            if (user != null)
            {
                // Create cookie
                var cookie = new HttpCookie("User");
                cookie.Values["UserName"] = signIn.Username;
                cookie.Values["UserId"] = user.UserId.ToString();
                cookie.Values["Role"] = user.Role.RoleId.ToString();
                // If remember me, keep the cookie for one year
                cookie.Expires.AddDays(signIn.RememberMe ? 365 : 1);

                HttpContext.Response.Cookies.Remove("User");
                HttpContext.Response.SetCookie(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "The user name or password provided is incorrect.");

            return View();
        }

        //
        // GET: /Account/SignOut
        public ActionResult SignOut()
        {
            // Delete the cookie
            var cookie = new HttpCookie("User") {Expires = DateTime.Now.AddDays(-1d)};
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel register)
        {
            if (!ModelState.IsValid) return View(register);
            // If username doesn't already exist
            var userExist = db.Users.FirstOrDefault(u => u.Username == register.Username);
            if (userExist == null)
            {
                // Hash password
                var password = CreatePasswordHash(register.Password);
                // Add user
                db.Users.Add(new User()
                {
                    Username = register.Username,
                    Password = password,
                    Name = register.Name,
                    LastName = register.LastName,
                    Email = register.Email,
                    RoleId = 3
                });
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "The user name already exist !");

            return View(register);
        }

        private static string CreatePasswordHash(string password)
        {
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            return hashedPwd;
        }
    }
}
