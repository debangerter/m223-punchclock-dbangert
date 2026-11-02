using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Services;

public class ExampleDataService(DatabaseContext databaseContext)
{
    public async Task CreateExampleDataAsync()
    {
        if (await databaseContext.Tags.AnyAsync()
            || await databaseContext.Categories.AnyAsync()
            || await databaseContext.Entries.AnyAsync())
            return;

        Category project = new() { Title = "Projektarbeit" };
        Category lesson = new() { Title = "Unterricht" };
        Category administration = new() { Title = "Administration" };
        databaseContext.Categories.AddRange(project, lesson, administration);

        Tag dotnet = new() { Title = ".NET" };
        Tag backend = new() { Title = "Backend" };
        Tag documentation = new() { Title = "Dokumentation" };
        databaseContext.Tags.AddRange(dotnet, backend, documentation);

        databaseContext.Entries.AddRange(
            new Entry
            {
                CheckIn = new DateTime(2026, 10, 12, 8, 0, 0, DateTimeKind.Utc),
                CheckOut = new DateTime(2026, 10, 12, 11, 30, 0, DateTimeKind.Utc),
                Category = project,
                Tags = [dotnet, backend]
            },
            new Entry
            {
                CheckIn = new DateTime(2026, 10, 12, 13, 0, 0, DateTimeKind.Utc),
                CheckOut = new DateTime(2026, 10, 12, 16, 0, 0, DateTimeKind.Utc),
                Category = lesson,
                Tags = [dotnet]
            },
            new Entry
            {
                CheckIn = new DateTime(2026, 10, 13, 8, 30, 0, DateTimeKind.Utc),
                CheckOut = new DateTime(2026, 10, 13, 9, 30, 0, DateTimeKind.Utc),
                Category = administration,
                Tags = [documentation]
            },
            new Entry
            {
                CheckIn = new DateTime(2026, 10, 13, 10, 0, 0, DateTimeKind.Utc),
                CheckOut = new DateTime(2026, 10, 13, 11, 0, 0, DateTimeKind.Utc)
            });

        await databaseContext.SaveChangesAsync();
    }
}