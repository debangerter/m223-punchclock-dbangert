using JetBrains.Annotations;
using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service;

[UsedImplicitly]
public sealed class EntryService(DatabaseContext databaseContext) : AbstractService<Entry>(databaseContext)
{
    protected override void UpdateModel(Entry existingModel, Entry model)
    {
        existingModel.CheckIn = model.CheckIn;
        existingModel.CheckOut = model.CheckOut;
    }

    protected override IQueryable<Entry> Includes(IQueryable<Entry> query)
    {
        return base.Includes(query).Include(entry => entry.Category);
    }
}