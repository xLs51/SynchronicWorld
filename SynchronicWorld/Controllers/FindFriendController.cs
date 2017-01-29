using System.Linq;
using System.Net;
using System.Web.Mvc;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class FindFriendController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        //
        // GET: /FindFriend/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /FindFriend/
        [HttpPost]
        public ActionResult Index(FindFriendModel model)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userName = "";
            if (cookie != null)
                userName = cookie["UserName"];

            // Get the name researched
            var search = model.FriendSearch;

            // Find the nickame or the firstname/lastname in the db
            var friends = model.TypeSearch == "Nickname" ? db.Users.SqlQuery(@"SELECT * FROM Users WHERE Username LIKE {0} AND Username <> {1}", "%"+search+"%", userName).ToList() : db.Users.SqlQuery(@"SELECT * FROM Users WHERE (Name LIKE {0} OR LastName LIKE {0}) AND Username <> {1}", "%" + search + "%", userName).ToList();

            model.ListUsers = friends;

            return View(model);
        }

        //
        // GET: /FindFriend/Add/1
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the friend, if he exist
            var hisfriend = db.Users.Find(id);

            if (hisfriend == null)
            {
                return HttpNotFound();
            }

            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userId = 0;
            if (cookie != null)
                userId = int.Parse(cookie["UserId"]);

            var me = db.Users.Find(userId);
            if (me == null) return View("FriendSucess");
            // If not already friend
            var friendExist = db.Friends.FirstOrDefault(f => f.UserId == me.UserId && f.HisFriendId == hisfriend.UserId);
            if (friendExist == null)
            {
                // Add the friend
                db.Friends.Add(new Friend()
                {
                    UserId = me.UserId,
                    HisFriendId = hisfriend.UserId,
                });
                db.SaveChanges();
            }
            else
                return View("FriendAlready");
            return View("FriendSucess");
        }

        //
        // GET: /FindFriend/FriendSucess
        public ActionResult FriendSucess()
        {
            return View();
        }

        //
        // GET: /FindFriend/FriendAlready
        public ActionResult FriendAlready()
        {
            return View();
        }
	}
}