using System;
using System.Collections.Generic;
using System.Linq;

namespace SynchronicWorld.Models
{
    public class SynchronicWorldInitializer
    {
        public static void Initialize()
        {
            var db = new SynchronicWorldDbContext();

            if (db.Roles.Any()) return;
            var roles = new List<Role>
            {
                new Role {RoleId = 1, Name = "Admin"},
                new Role {RoleId = 2, Name = "WebSite Admin"},
                new Role {RoleId = 3, Name = "User"}
            };
            roles.ForEach(r => db.Roles.Add(r));
            db.SaveChanges();

            if (db.Users.Any()) return;
            var users = new List<User>
            {
                /* pwd : admin51 */new User{UserId = 1, Username = "admin",Password = "33380225B59D05E6A00106C1DE0EBB6DF0399ECB",Name = "Admin",LastName = "Admin",Email = "admin@supinfo.com",RoleId = 1}, 
                /* pwd : jean51 */new User{UserId = 2, Username = "jean51",Password = "8076772490117AB5EF0B165A1EDF82BCACD0D6ED",Name = "Jean",LastName = "Alain",Email = "jean.alin@supinfo.com",RoleId = 3},
                /* pwd : toto51 */new User{UserId = 3, Username = "toto51",Password = "14455D65DEC5AD82717FF9FBB4012775E28B68E3",Name = "Thomas",LastName = "Tim",Email = "thomas.tim@supinfo.com",RoleId = 3},

            };
            users.ForEach(u => db.Users.Add(u));
            db.SaveChanges();

            if (db.TypeEvents.Any()) return;
            var typeevents = new List<TypeEvent>
            {
                new TypeEvent {TypeEventId = 1, Name = "Party"},
                new TypeEvent {TypeEventId = 2, Name = "Lunch"}
            };
            typeevents.ForEach(c => db.TypeEvents.Add(c));
            db.SaveChanges();

            if (db.Events.Any()) return;
            var events = new List<Event>
            {
                new Event {EventId = 1, Name = "Fête de la bière", Address = "17 Rue des cerisiers", Description = "Fête de la bière", Date = new DateTime(2014,7,23,20,30,0), TypeEventId = 1, Status = "Open", CreatorId = 1},
                new Event {EventId = 2, Name = "Fête du chocolat", Address = "17 Rue des fleurs", Description = "Fête du chocolat", Date = new DateTime(2014,7,23,20,30,0), TypeEventId = 1, Status = "Pending", CreatorId = 3}
            };
            events.ForEach(e => db.Events.Add(e));
            db.SaveChanges();

            if (db.UserEvents.Any()) return;
            var userevents = new List<UserEvent>
            {
                new UserEvent {EventId = 1, UserId = 1},
                new UserEvent {EventId = 1, UserId = 2},
                new UserEvent {EventId = 1, UserId = 3},
                new UserEvent {EventId = 2, UserId = 1},
                new UserEvent {EventId = 2, UserId = 3},
            };
            userevents.ForEach(ue => db.UserEvents.Add(ue));
            db.SaveChanges();

            if (db.TypeContributions.Any()) return;
            var typecontributions = new List<TypeContribution>
            {
                new TypeContribution {TypeContributionId = 1, Name = "Money"},
                new TypeContribution {TypeContributionId = 2, Name = "Food"},
                new TypeContribution {TypeContributionId = 3, Name = "Beverage"}
            };
            typecontributions.ForEach(tc => db.TypeContributions.Add(tc));
            db.SaveChanges();

            if (db.Contributions.Any()) return;
            var contributions = new List<Contribution>
            {
                new Contribution {Name = "Coca Cola", Quantity = "3", TypeContributionId = 2, UserId = 3, EventId = 1}
            };
            contributions.ForEach(c => db.Contributions.Add(c));
            db.SaveChanges();
        }
    }
}