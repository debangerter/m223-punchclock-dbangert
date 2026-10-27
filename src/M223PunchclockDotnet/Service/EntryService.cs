using JetBrains.Annotations;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Model.Dto;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            EntityEntry<Entry> entityEntry = databaseContext.Entries.Add(entry);
            await databaseContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task<bool> DeleteEntry(int id)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return false;
            
            databaseContext.Entries.Remove(entry);
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<Entry?> UpdateEntry(int id, Entry entry)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } existingEntry) return null;

            existingEntry.CheckIn = entry.CheckIn;
            existingEntry.CheckOut = entry.CheckOut;

            await databaseContext.SaveChangesAsync();

            return existingEntry;
        }

        public async Task<Entry?> UpdateEntry(int id, JsonPatchDocument<Entry> entryPatch)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return null;

            entryPatch.ApplyTo(entry);
            
            await databaseContext.SaveChangesAsync();

            return entry;
        }
    }
}