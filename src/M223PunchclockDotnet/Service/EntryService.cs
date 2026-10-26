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
        public async Task<List<EntryDto>> FindAll()
        {
            List<Entry> entries = await databaseContext.Entries.ToListAsync();
            
            return [.. entries.Select(EntryDto.FromModel)];
        }

        public async Task<EntryDto> AddEntry(EntryDto entry)
        {
            EntityEntry<Entry> newEntry = databaseContext.Entries.Add(entry.ToModel());
            await databaseContext.SaveChangesAsync();

            return EntryDto.FromModel(newEntry.Entity);
        }

        public async Task<bool> DeleteEntry(int id)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return false;
            
            databaseContext.Entries.Remove(entry);
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<EntryDto?> UpdateEntry(int id, EntryDto entry)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } existingEntry) return null;

            existingEntry.CheckIn = entry.CheckIn!.Value;
            existingEntry.CheckOut = entry.CheckOut!.Value;

            await databaseContext.SaveChangesAsync();

            return EntryDto.FromModel(existingEntry);
        }

        public async Task<EntryDto?> UpdateEntry(int id, JsonPatchDocument<Entry> entry)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } existingEntry) return null;

            entry.ApplyTo(existingEntry);
            
            await databaseContext.SaveChangesAsync();

            return EntryDto.FromModel(existingEntry);
        }
    }
}