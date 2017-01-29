using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class EventController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        // GET: /Event/
        public ActionResult Index()
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            // Display the events in which he participates
            var myevent = db.UserEvents.SqlQuery(@"SELECT * FROM UserEvents WHERE UserId = {0}", userid).ToList();
            var listevent = myevent.Select(e => db.Events.FirstOrDefault(ev => ev.EventId == e.EventId)).ToList();

            return View(listevent);
        }

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the event
            var @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            // Display the event and its contributions
            var contributions = db.Contributions.SqlQuery(@"SELECT * FROM Contributions WHERE EventId = {0}", id).ToList();
            var ecm = new EventContributionModel {Event = @event, Contributions = contributions};
            
            return View(ecm);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            ViewBag.TypeEventId = new SelectList(db.TypeEvents, "TypeEventId", "Name");
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EventId,Name,Address,Description,Date,Type,Status,CreatorId")] Event @event)
        {
            if (!ModelState.IsValid) return View(@event);
            // Add the event
            db.Events.Add(@event);
            db.UserEvents.Add(new UserEvent()
            {
                UserId = @event.CreatorId,
                EventId= @event.EventId
            });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(int? id)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // He can only edit the event if he is the creator
            var myevent = db.Events.FirstOrDefault(e => e.EventId == id && e.CreatorId == userid);
            if (myevent == null)
            {
                return HttpNotFound();
            }

            // Find the event
            var @event = db.Events.Find(id);

            if (@event == null)
            {
                return HttpNotFound();
            }

            ViewBag.TypeEventId = new SelectList(db.TypeEvents, "TypeEventId", "Name", @event.TypeEventId);
            return View(@event);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EventId,Name,Address,Description,Date,Type,Status,CreatorId")] Event @event)
        {
            if (!ModelState.IsValid) return View(@event);
            // Edit the event
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // He can only delete the event if he is the creator
            var myevent = db.Events.FirstOrDefault(e => e.EventId == id && e.CreatorId == userid);
           
            if (myevent == null)
            {
                return HttpNotFound();
            }

            // Find the event
            var @event = db.Events.Find(id);
            
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Get all the participant of that event
            var myevent = db.UserEvents.SqlQuery(@"SELECT * FROM UserEvents WHERE EventId = {0}", id).ToList();

            // Remove all the participant of that event
            foreach (var ue in myevent)
                db.UserEvents.Remove(ue);

            // Find the event
            var @event = db.Events.Find(id);
            // Remove the event
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Event/InviteFriend/5
        public ActionResult InviteFriend(int? id)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If he participates to the event
            var myevent = db.UserEvents.FirstOrDefault(ue => ue.EventId == id && ue.UserId == userid);
            
            if (myevent == null)
            {
                return HttpNotFound();
            }

            // We can only invite friends if the event is open
            var events = db.Events.FirstOrDefault(e => e.EventId == id && e.Status == "Open");
            
            if (events == null)
            {
                return HttpNotFound();
            }

            // Find all his friends
            var myfriend = db.Friends.SqlQuery(@"SELECT * FROM Friends WHERE UserId = {0}", userid).ToList();

            var model = new InviteFriendModel { ListUsersInvited = new List<UserInvited>() };
            // Add each friends to a list if they are not already invited to the event
            foreach (var inviteduser in from f in myfriend let userAlreadyInvited = db.UserEvents.FirstOrDefault(ue => ue.EventId == id && ue.UserId == f.HisFriendId) where userAlreadyInvited == null select db.Users.FirstOrDefault(u => u.UserId == f.HisFriendId) into user select new UserInvited
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                LastName = user.LastName
            })
            {
                model.ListUsersInvited.Add(inviteduser);
            }

            return View(model);
        }

        // POST: /Event/InviteFriend/5
        [HttpPost]
        public ActionResult InviteFriend(int? id, List<UserInvited> model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Add each friends as a participant if they are not already invited
            foreach (var userevent in from u in model where u.Invited let userAlreadyInvited = db.UserEvents.FirstOrDefault(ue => ue.EventId == id && ue.UserId == u.UserId) where userAlreadyInvited == null select new UserEvent {UserId = u.UserId, EventId = (int) id})
            {
                db.UserEvents.Add(userevent);
            }
            db.SaveChanges();

            return View("InviteFriendSucess");
        }

        // GET: /Event/Participant/5
        public ActionResult Participant(int? id)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If he participates to the event
            var myevent = db.UserEvents.FirstOrDefault(ue => ue.EventId == id && ue.UserId == userid);
            if (myevent == null)
            {
                return HttpNotFound();
            }

            // Find all the participant
            var userevent = db.UserEvents.SqlQuery(@"SELECT * FROM UserEvents WHERE EventId = {0}", id).ToList();
            var model = new MyFriendModel {ListUsers = new List<User>()};
            // Add each participant to a list
            foreach (var ue in userevent)
            {
                model.ListUsers.Add(db.Users.FirstOrDefault(u => u.UserId == ue.UserId));
            }

            return View(model);
        }

        // GET: /FindFriend/InviteFriendSucess
        public ActionResult InviteFriendSucess()
        {
            return View();
        }

        // GET: /Contribution/AddContribution/5
        public ActionResult AddContribution(int? id)
        {
            // Get the id in the cookie
            var cookie = Request.Cookies["User"];
            var userid = 0;
            if (cookie != null)
                userid = int.Parse(cookie["UserId"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If he participates to the event
            var myevent = db.UserEvents.FirstOrDefault(ue => ue.EventId == id && ue.UserId == userid);
            if (myevent == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventId = id;
            ViewBag.TypeContributionId = new SelectList(db.TypeContributions, "TypeContributionId", "Name");
            return View();
        }

        // POST: /Contribution/AddContribution/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContribution([Bind(Include = "ContributionId,Name,Quantity,TypeContributionId,UserId,EventId")] Contribution contribution)
        {
            if (!ModelState.IsValid) return View("Index");
            // Add the contribution
            db.Contributions.Add(contribution);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
