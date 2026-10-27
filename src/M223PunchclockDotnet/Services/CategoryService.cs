using JetBrains.Annotations;
using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;

namespace M223PunchclockDotnet.Services;

[UsedImplicitly]
public sealed class CategoryService(DatabaseContext databaseContext) : AbstractServiceWithCrud<Category>(databaseContext)
{
    protected override void UpdateModel(Category existingModel, Category model)
    {
        existingModel.Title = model.Title;
        existingModel.Entries = model.Entries;
    }
}