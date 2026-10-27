using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Model;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }


    public DbSet<Entry> Entries { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entry>()
            .ToTable(nameof(Entry))
            .HasOne(entry => entry.Category)
            .WithMany(category => category.Entries)
            .HasForeignKey(entry => entry.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>()
            .ToTable(nameof(Category));
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=db;Database=postgres;Username=postgres;Password=postgres");
    }
}