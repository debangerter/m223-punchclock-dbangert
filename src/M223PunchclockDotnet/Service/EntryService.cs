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
            
            return [.. entries.Select(entry => new EntryDto(entry))];
        }

        public async Task<EntryDto> AddEntry(EntryDto entryDto)
        {
            EntityEntry<Entry> entityEntry = databaseContext.Entries.Add(entryDto.ToModel());
            await databaseContext.SaveChangesAsync();

            return new EntryDto(entityEntry.Entity);
        }

        public async Task<bool> DeleteEntry(int id)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return false;
            
            databaseContext.Entries.Remove(entry);
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<EntryDto?> UpdateEntry(int id, EntryDto entryDto)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return null;

            entry.CheckIn = entryDto.CheckIn!.Value;
            entry.CheckOut = entryDto.CheckOut!.Value;

            await databaseContext.SaveChangesAsync();

            return new EntryDto(entry);
        }

        public async Task<EntryDto?> UpdateEntry(int id, JsonPatchDocument<Entry> entryPatch)
        {
            if (await databaseContext.Entries.FindAsync(id) is not { } entry) return null;

            entryPatch.ApplyTo(entry);
            
            await databaseContext.SaveChangesAsync();

            return new EntryDto(entry);
        }
    }
}