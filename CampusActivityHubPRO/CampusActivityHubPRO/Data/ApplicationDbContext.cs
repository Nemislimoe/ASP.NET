using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusActivityHubPRO.Data;

namespace CampusActivityHubPRO.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<EventTag> EventTags => Set<EventTag>();
        public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // EventTag composite key
            builder.Entity<EventTag>()
                .HasKey(et => new { et.EventId, et.TagId });

            // Relations
            builder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ❌ Исправлено: делаем связи Event необязательными для EventTag и Registration
            builder.Entity<EventTag>()
                .HasOne(et => et.Event)
                .WithMany(e => e.EventTags)
                .IsRequired(false);

            builder.Entity<EventTag>()
                .HasOne(et => et.Tag)
                .WithMany()
                .HasForeignKey(et => et.TagId);

            builder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .IsRequired(false);

            builder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Global query filter for soft delete
            builder.Entity<Event>().HasQueryFilter(e => !e.IsDeleted);

            // Seed minimal tags (optional)
            builder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "Workshop" },
                new Tag { Id = 2, Name = "Seminar" },
                new Tag { Id = 3, Name = "Competition" }
            );
        }

        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Roles
            var roles = new[] { "Admin", "Organizer", "Student" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }

            // Admin
            var adminEmail = "admin@campushub.local";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, Name = "System Admin", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Organizers
            var org1Email = "org1@campushub.local";
            var org2Email = "org2@campushub.local";

            if (await userManager.FindByEmailAsync(org1Email) == null)
            {
                var o1 = new ApplicationUser { UserName = org1Email, Email = org1Email, Name = "Organizer One", EmailConfirmed = true };
                await userManager.CreateAsync(o1, "Organizer123!");
                await userManager.AddToRoleAsync(o1, "Organizer");
            }
            if (await userManager.FindByEmailAsync(org2Email) == null)
            {
                var o2 = new ApplicationUser { UserName = org2Email, Email = org2Email, Name = "Organizer Two", EmailConfirmed = true };
                await userManager.CreateAsync(o2, "Organizer123!");
                await userManager.AddToRoleAsync(o2, "Organizer");
            }

            // Categories
            if (!db.Categories.Any())
            {
                db.Categories.AddRange(
                    new Category { Name = "Academic", Description = "Lectures, seminars, academic events", IconClass = "bi-book" },
                    new Category { Name = "Sports", Description = "Sporting events and competitions", IconClass = "bi-trophy" },
                    new Category { Name = "Culture", Description = "Cultural and social events", IconClass = "bi-music-note" }
                );
                await db.SaveChangesAsync();
            }

            // Events (5 test events)
            if (!db.Events.Any())
            {
                var organizer1 = await userManager.FindByEmailAsync(org1Email);
                var organizer2 = await userManager.FindByEmailAsync(org2Email);
                var catAcademic = db.Categories.First(c => c.Name == "Academic").Id;
                var catSports = db.Categories.First(c => c.Name == "Sports").Id;
                var catCulture = db.Categories.First(c => c.Name == "Culture").Id;

                db.Events.AddRange(
                    new Event { Title = "Math Seminar", Description = "Advanced topics", StartAt = DateTime.UtcNow.AddDays(7).AddHours(10), EndAt = DateTime.UtcNow.AddDays(7).AddHours(12), Capacity = 50, CategoryId = catAcademic, OrganizerId = organizer1.Id },
                    new Event { Title = "Football Friendly", Description = "Interfaculty match", StartAt = DateTime.UtcNow.AddDays(10).AddHours(15), EndAt = DateTime.UtcNow.AddDays(10).AddHours(17), Capacity = 22, CategoryId = catSports, OrganizerId = organizer2.Id },
                    new Event { Title = "Choir Night", Description = "Evening of songs", StartAt = DateTime.UtcNow.AddDays(14).AddHours(18), EndAt = DateTime.UtcNow.AddDays(14).AddHours(20), Capacity = 100, CategoryId = catCulture, OrganizerId = organizer1.Id },
                    new Event { Title = "Programming Workshop", Description = "Hands-on C#", StartAt = DateTime.UtcNow.AddDays(5).AddHours(9), EndAt = DateTime.UtcNow.AddDays(5).AddHours(13), Capacity = 30, CategoryId = catAcademic, OrganizerId = organizer2.Id },
                    new Event { Title = "Chess Tournament", Description = "Open tournament", StartAt = DateTime.UtcNow.AddDays(20).AddHours(11), EndAt = DateTime.UtcNow.AddDays(20).AddHours(18), Capacity = 64, CategoryId = catCulture, OrganizerId = organizer1.Id }
                );
                await db.SaveChangesAsync();
            }
        }
    }
}
