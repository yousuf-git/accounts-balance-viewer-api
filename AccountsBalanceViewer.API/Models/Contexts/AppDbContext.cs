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
        // set unique indexes
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
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
    
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        _SetTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        _SetTimestamps();
        return base.SaveChanges();
    }
    
    private void _SetTimestamps()
    {
        // set CreatedAt and UpdatedAt behaviors
        var entityEntries = ChangeTracker
            .Entries()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entityEntries)
        {
            entityEntry.Property("UpdatedAt").CurrentValue = DateTime.Now.ToUniversalTime();

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property("CreatedAt").CurrentValue = DateTime.Now.ToUniversalTime();
            }
        }
    }
}