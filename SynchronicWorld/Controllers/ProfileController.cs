using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class ProfileController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        //
        // GET: /Profile/
        public ActionResult Index()
        {
            // Get the username in the cookie
            var cookie = Request.Cookies["User"];
            var userName = "";
            if (cookie != null)
                userName = cookie["UserName"];

            // Find the user
            var user = db.Users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
                return RedirectToAction("Index", "Home");

            var editModel = new EditModel {UserId = user.UserId, Name = user.Name, LastName = user.LastName, Email = user.Email};
            return View(editModel);
        }

        //
        // POST: /Profile/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EditModel model)
        {
            // Find the user
            var userSearch = db.Users.Find(model.UserId);

            if (!ModelState.IsValid) return View(model);
            // Hash password
            var oldPassword = CreatePasswordHash(model.OldPassword);
            // If old password isn't the same than the current one
            if (oldPassword != userSearch.Password)
                ModelState.AddModelError("", "The current password provided is incorrect.");
            else
            {
                // Hash password
                var password = CreatePasswordHash(model.NewPassword);
                userSearch.Name = model.Name;
                userSearch.LastName = model.LastName;
                userSearch.Email = model.Email;
                userSearch.Password = password;

                // Edit the user profile
                db.Entry(userSearch).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("EditProfileSucess");
            }

            return View(model);
        }

        //
        // GET: /Profile/EditProfileSucess
        public ActionResult EditProfileSucess()
        {
            return View();
        }

        private static string CreatePasswordHash(string password)
        {
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            return hashedPwd;
        }
	}
}