using Microsoft.EntityFrameworkCore;
using PetManagement.Entities;

namespace PetManagement.Database;

public class PetManagementDbContext : DbContext
{
    public PetManagementDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>()
            .HasOne(p => p.HealthStatus)
            .WithOne(hs => hs.Pet)
            .HasForeignKey<HealthStatus>(hs => hs.PetId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<HealthStatus> HealthStatuses { get; set; }
    public DbSet<SocialInteraction> SocialInteractions { get; set; }
    public DbSet<Training> Trainings { get; set; }
}
