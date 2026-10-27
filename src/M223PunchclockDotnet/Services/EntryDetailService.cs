using JetBrains.Annotations;
using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Services;

[UsedImplicitly]
public sealed class EntryDetailService(DatabaseContext databaseContext) : IService<EntryDetail>
{
    public async Task<List<EntryDetail>> FindAll()
    {
        List<Entry> entries = await databaseContext.Entries
            .Include(entry => entry.Category)
            .Include(entry => entry.Tags)
            .ToListAsync();

        return
        [
            .. entries.Select(entry => new EntryDetail
            {
                Id = entry.Id,
                CheckIn = entry.CheckIn,
                CheckOut = entry.CheckOut,
                Category = entry.Category,
                Tags = entry.Tags
            })
        ];
    }

    public async Task<EntryDetail?> FindById(int id)
    {
        Entry? entry = await databaseContext.Entries
            .Include(entry => entry.Category)
            .Include(entry => entry.Tags)
            .FirstOrDefaultAsync(entry => entry.Id == id);

        if (entry is null) return null;

        return new EntryDetail
        {
            Id = entry.Id,
            CheckIn = entry.CheckIn,
            CheckOut = entry.CheckOut,
            Category = entry.Category,
            Tags = entry.Tags
        };
    }

    public Task<EntryDetail> Add(EntryDetail model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<EntryDetail?> Update(int id, EntryDetail model)
    {
        throw new NotImplementedException();
    }

    public Task<EntryDetail?> Update(int id, JsonPatchDocument<EntryDetail> modelPatch)
    {
        throw new NotImplementedException();
    }
}