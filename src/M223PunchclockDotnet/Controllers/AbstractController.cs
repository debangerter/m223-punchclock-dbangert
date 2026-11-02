using System.Net.Mime;
using System.Reflection;
using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[ApiController]
public abstract class AbstractController<TDto, TModel>(IService<TModel> service) : ControllerBase
    where TModel : AbstractDbModel
    where TDto : AbstractDto<TModel>
{
    private static readonly ConstructorInfo? DtoConstructor = typeof(TDto).GetConstructor([typeof(TModel)]);

    private static readonly Func<TModel, TDto> GetDto = model => DtoConstructor is null
        ? throw new Exception($"{typeof(TDto).Name} does not have a constructor that takes {typeof(TModel).Name}")
        : DtoConstructor.Invoke([model]) as TDto
          ?? throw new Exception($"{typeof(TDto).Name}'s constructor returns null");

    [HttpGet]
    public virtual async Task<IActionResult> GetModels()
    {
        List<TModel> models = await service.FindAll();

        return Ok(models.Select(model => GetDto(model)));
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public virtual async Task<IActionResult> AddModel([FromBody] TDto modelDto)
    {
        TModel newModel = await service.Add(modelDto.ToModel());
        
        await OnBeforeAddModelAsync(modelDto, newModel);
        
        TDto newModelDto = GetDto(newModel);
        return CreatedAtAction(nameof(GetModels), new { id = newModelDto.Id }, newModelDto);
    }

    protected virtual Task OnBeforeAddModelAsync(TDto modelDto, TModel model)
    {
        return Task.CompletedTask;
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteModel(int id)
    {
        bool successful = await service.Delete(id);
        return successful ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> UpdateEntry(int id, [FromBody] TDto modelDto)
    {
        TModel modelToUpdate = modelDto.ToModel();
        
        OnBeforeUpdateModel(ref modelToUpdate);
        
        TModel? updatedModel = await service.Update(id, modelToUpdate);

        return updatedModel is null ? NotFound() : Ok(GetDto(updatedModel));
    }
    
    protected virtual void OnBeforeUpdateModel(ref TModel model)
    {
    }

    [HttpPatch("{id:int}")]
    [Consumes(MediaTypeNames.Application.JsonPatch)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> PatchEntry(int id, [FromBody] JsonPatchDocument<TModel> modelPatch)
    {
        TModel? updatedModel = await service.Update(id, modelPatch);

        return updatedModel is null ? NotFound() : Ok(GetDto(updatedModel));
    }
}