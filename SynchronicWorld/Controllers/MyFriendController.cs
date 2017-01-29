using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class MyFriendController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        //
        // GET: /MyFriend/
        public ActionResult Index()
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userId = "";
            if (cookie != null)
                userId = cookie["UserId"];

            // Display all his friends
            var allfriends = db.Friends.SqlQuery(@"SELECT * FROM Friends WHERE UserId = {0}", userId).ToList();

            // Add them to a list
            var model = new MyFriendModel {ListUsers = new List<User>()};
            model.ListUsers.AddRange(allfriends.Select(friend => db.Users.Find(friend.HisFriendId)));

            return View(model);
        }

        //
        // GET: /MyFriend/Delete/1
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userId = 0;
            if (cookie != null)
                userId = int.Parse(cookie["UserId"]);

            // If they are really friend
            var friend = db.Friends.FirstOrDefault(f => f.UserId == userId && f.HisFriendId == id);

            if (friend == null)
            {
                return HttpNotFound();
            }

            // Remove the friend
            db.Friends.Remove(friend);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
	}
}