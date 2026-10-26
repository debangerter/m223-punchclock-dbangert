using JetBrains.Annotations;
using M223PunchclockDotnet.Model;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    [UsedImplicitly]
    public class EntryService(DatabaseContext databaseContext)
    {
        public Task<List<Entry>> FindAll()
        {
            return databaseContext.Entries.ToListAsync();
        }

        public async Task<Entry> AddEntry(Entry entry)
        {
            databaseContext.Entries.Add(entry);
            await databaseContext.SaveChangesAsync();

            return entry;
        }

        public async Task DeleteEntry(int id)
        {
            Entry? entry = await databaseContext.Entries.FindAsync(id);
            if (entry is null) return;
            databaseContext.Entries.Remove(entry);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<Entry?> UpdateEntry(int id, Entry entry)
        {
            Entry? existingEntry = await databaseContext.Entries.FindAsync(id);

            if (existingEntry is null) return null;

            existingEntry.Id = entry.Id;
            existingEntry.CheckIn = entry.CheckIn;
            existingEntry.CheckOut = entry.CheckOut;

            await databaseContext.SaveChangesAsync();

            return existingEntry;
        }

        public async Task<Entry?> UpdateEntry(int id, JsonPatchDocument<Entry> entry)
        {
            Entry? existingEntry = await databaseContext.Entries.FindAsync(id);

            if (existingEntry is null) return null;

            databaseContext.Attach(existingEntry);
            entry.ApplyTo(existingEntry);
            
            await databaseContext.SaveChangesAsync();

            return existingEntry;
        }
    }
}