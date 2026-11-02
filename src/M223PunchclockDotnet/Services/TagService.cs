using JetBrains.Annotations;
using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Services;

[UsedImplicitly]
public sealed class TagService(DatabaseContext databaseContext) : AbstractServiceWithCrud<Tag>(databaseContext)
{
    protected override void UpdateModel(Tag existingModel, Tag model)
    {
        existingModel.Title = model.Title;
        existingModel.Entries = model.Entries;
    }

    protected override IQueryable<Tag> Includes(IQueryable<Tag> query)
    {
        return base.Includes(query).Include(tag => tag.Entries);
    }
}