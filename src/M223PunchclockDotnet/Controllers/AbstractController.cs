using System.Net.Mime;
using System.Reflection;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Model.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[ApiController]
public abstract class AbstractController<TDto, TService, TModel>(TService service) : ControllerBase
    where TModel : AbstractDbModel
    where TDto : AbstractDto<TModel>
    where TService : AbstractService<TModel>
{
    private static readonly ConstructorInfo? DtoConstructor = typeof(TDto).GetConstructor([typeof(TModel)]);

    private static readonly Func<TModel, TDto> GetDto = model => DtoConstructor is null
        ? throw new Exception($"{typeof(TDto).Name} does not have a constructor that takes {typeof(TModel).Name}")
        : DtoConstructor.Invoke([model]) as TDto
          ?? throw new Exception($"{typeof(TDto).Name}'s constructor returns null");

    [HttpGet]
    public virtual async Task<IActionResult> GetModel()
    {
        List<TModel> models = await service.FindAll();

        return Ok(models.Select(model => GetDto?.Invoke(model)));
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public virtual async Task<ActionResult<TDto>> AddModel([FromBody] TDto modelDto)
    {
        TModel newModel = await service.Add(modelDto.ToModel());
        TDto newModelDto = GetDto(newModel);
        return CreatedAtAction(typeof(TModel).Name, new { id = newModelDto.Id }, newModelDto);
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
        TModel? updatedModel = await service.Update(id, modelDto.ToModel());

        return updatedModel is null ? NotFound() : Ok(GetDto(updatedModel));
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