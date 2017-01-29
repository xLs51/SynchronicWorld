using System.Data.Entity;

namespace SynchronicWorld.Models
{
    public class SynchronicWorldDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TypeEvent> TypeEvents { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<TypeContribution> TypeContributions { get; set; }
        public SynchronicWorldDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SynchronicWorldDbContext>());
        }
    }
}