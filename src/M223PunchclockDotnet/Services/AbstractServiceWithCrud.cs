using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace M223PunchclockDotnet.Services;

public abstract class AbstractServiceWithCrud<TModel>(DatabaseContext databaseContext) : IService<TModel> 
    where TModel : AbstractDbModel
{
    public Task<List<TModel>> FindAll()
    {
        IQueryable<TModel> query = databaseContext.Set<TModel>().AsQueryable();
        return Includes(query).ToListAsync();
    }

    public Task<TModel?> FindById(int id)
    {
        IQueryable<TModel> query = databaseContext.Set<TModel>().AsQueryable();
        return Includes(query).FirstOrDefaultAsync(model => model.Id == id);
    }

    public virtual async Task<TModel> Add(TModel model)
    {
        EntityEntry<TModel> entityEntry = databaseContext.Set<TModel>().Add(model);
        await databaseContext.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        if (await GetModelAsync(id) is not { } model) return false;

        databaseContext.Set<TModel>().Remove(model);
        await databaseContext.SaveChangesAsync();

        return true;
    }

    public async Task<TModel?> Update(int id, TModel model)
    {
        if (await GetModelAsync(id) is not { } existingModel) return null;

        UpdateModel(existingModel, model);

        await databaseContext.SaveChangesAsync();

        return existingModel;
    }

    protected abstract void UpdateModel(TModel existingModel, TModel model);

    public async Task<TModel?> Update(int id, JsonPatchDocument<TModel> modelPatch)
    {
        if (await GetModelAsync(id) is not { } model) return null;

        modelPatch.ApplyTo(model);

        await databaseContext.SaveChangesAsync();

        return model;
    }

    private Task<TModel?> GetModelAsync(int id)
    {
        IQueryable<TModel> query = databaseContext.Set<TModel>().AsQueryable();

        return Includes(query).FirstOrDefaultAsync(x => x.Id == id);
    }

    protected virtual IQueryable<TModel> Includes(IQueryable<TModel> query)
    {
        return query;
    }
}