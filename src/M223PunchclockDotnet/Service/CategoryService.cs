using JetBrains.Annotations;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Service;

[UsedImplicitly]
public sealed class CategoryService(DatabaseContext databaseContext) : AbstractService<Category>(databaseContext)
{
    protected override void UpdateModel(Category existingModel, Category model)
    {
        existingModel.Title = model.Title;
    }
}