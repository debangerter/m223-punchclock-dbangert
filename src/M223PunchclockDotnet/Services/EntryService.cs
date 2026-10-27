using JetBrains.Annotations;
using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;

namespace M223PunchclockDotnet.Services;

[UsedImplicitly]
public sealed class EntryService(DatabaseContext databaseContext) : AbstractServiceWithCrud<Entry>(databaseContext)
{
    public override Task<Entry> Add(Entry model)
    {
        
        
        return base.Add(model);
    }

    protected override void UpdateModel(Entry existingModel, Entry model)
    {
        existingModel.CheckIn = model.CheckIn;
        existingModel.CheckOut = model.CheckOut;
    }
}