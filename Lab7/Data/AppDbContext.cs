using Microsoft.EntityFrameworkCore;
using Lab7.Models;

namespace Lab7.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Participant> Participants => Set<Participant>();
        public DbSet<EventParticipant> EventParticipants => Set<EventParticipant>();

        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<DishCategory> DishCategories => Set<DishCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order).WithMany(o => o.OrderProducts).HasForeignKey(op => op.OrderId);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product).WithMany(p => p.OrderProducts).HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.ParticipantId });
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event).WithMany(e => e.EventParticipants).HasForeignKey(ep => ep.EventId);
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Participant).WithMany(p => p.EventParticipants).HasForeignKey(ep => ep.ParticipantId);

            modelBuilder.Entity<DishCategory>()
                .HasKey(dc => new { dc.DishId, dc.CategoryId });
            modelBuilder.Entity<DishCategory>()
                .HasOne(dc => dc.Dish).WithMany(d => d.DishCategories).HasForeignKey(dc => dc.DishId);
            modelBuilder.Entity<DishCategory>()
                .HasOne(dc => dc.Category).WithMany(c => c.DishCategories).HasForeignKey(dc => dc.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
