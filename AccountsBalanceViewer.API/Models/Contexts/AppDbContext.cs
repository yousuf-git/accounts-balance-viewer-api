using AccountsViewer.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Models.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // setup CreatedBy relationship from Entry -> User
        modelBuilder.Entity<Entry>()
            .HasOne(e => e.User)
            .WithMany(u => u.Entries)
            .HasForeignKey(e => e.CreatedBy);

        // set CreatedAt and UpdatedAt shadow properties to all the entities
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.AddProperty("CreatedAt", typeof(DateTime));
            entity.AddProperty("UpdatedAt", typeof(DateTime));
        }
    }

    public override int SaveChanges()
    {
        // configure CreatedAt and UpdatedAt behaviors
        var entityEntries = ChangeTracker
            .Entries()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entityEntries)
        {
            entityEntry.Property("UpdatedAt").CurrentValue = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property("CreatedAt").CurrentValue = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }
}