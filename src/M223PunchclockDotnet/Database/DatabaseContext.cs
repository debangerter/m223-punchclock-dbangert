using M223PunchclockDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Tag> Tags { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .ToTable(nameof(Category));

        modelBuilder.Entity<Entry>()
            .ToTable(nameof(Entry))
            .HasOne(entry => entry.Category)
            .WithMany(category => category.Entries)
            .HasForeignKey(entry => entry.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Entry>()
            .HasMany(entry => entry.Tags)
            .WithMany(tag => tag.Entries)
            .UsingEntity("Entry_Tag",
                tag => tag.HasOne(typeof(Tag))
                    .WithMany()
                    .HasForeignKey($"{nameof(Tag)}Id")
                    .OnDelete(DeleteBehavior.Cascade),
                entry => entry.HasOne(typeof(Entry))
                    .WithMany()
                    .HasForeignKey($"{nameof(Entry)}Id")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        modelBuilder.Entity<Tag>()
            .ToTable(nameof(Tag));
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=db;Database=punchclock;Username=postgres;Password=postgres");
    }
}