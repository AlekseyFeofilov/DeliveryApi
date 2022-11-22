using DeliveryAppAPI.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.DbContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishBasket> DishBaskets { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
    }
}