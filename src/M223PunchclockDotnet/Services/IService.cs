using M223PunchclockDotnet.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace M223PunchclockDotnet.Services;

public interface IService<TModel> where TModel : AbstractDbModel
{
    Task<List<TModel>> FindAll();
    Task<TModel?> FindById(int id);
    Task<TModel> Add(TModel model);
    Task<bool> Delete(int id);
    Task<TModel?> Update(int id, TModel model);
    Task<TModel?> Update(int id, JsonPatchDocument<TModel> modelPatch);
}