using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SynchronicWorld.Models;

namespace SynchronicWorld.Controllers
{
    public class EventAdminController : Controller
    {
        private SynchronicWorldDbContext db = new SynchronicWorldDbContext();

        //
        // GET: /EventAdmin/
        public ActionResult Index()
        {
            var listevent = db.Events.SqlQuery(@"SELECT * FROM Events").ToList();
            return View(listevent);
        }

        //
        // GET: /Validate/
        public ActionResult Validate(int? id)
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

            // Validate it
            @event.Status = "Open";
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return View("ValidateSucess");
        }

        //
        // GET: /ValidateSucess
        public ActionResult ValidateSucess()
        {
            return View();
        }

        //
        // GET: /Lock/
        public ActionResult Lock(int? id)
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

            // Lock it
            @event.Status = "Lock";
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return View("LockSucess");
        }

        //
        // GET: /LockSucess
        public ActionResult LockSucess()
        {
            return View();
        }

        //
        // GET: /Unlock/
        public ActionResult Unlock(int? id)
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

            // Unlock it
            @event.Status = "Open";
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return View("UnlockSucess");
        }

        //
        // GET: /UnlockSucess
        public ActionResult UnlockSucess()
        {
            return View();
        }
	}
}