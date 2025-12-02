using CampusActivityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Event -> Category (Many-to-One)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event -> Organizer (User) (Many-to-One)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Registration relationships (Many-to-One to Event and User)
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .IsRequired(false)               // зробити optional
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes and constraints (examples)
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique(false);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.StartAt);

            // Global query filter for soft-delete on Events
            modelBuilder.Entity<Event>().HasQueryFilter(e => !e.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }
    }
}
