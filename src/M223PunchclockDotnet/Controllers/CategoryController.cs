using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[Route("categories")]
public sealed class CategoryController(IService<Category> service)
    : AbstractController<CategoryDto, Category>(service)
{
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> GetModels()
    {
        return base.GetModels();
    }

    [ProducesResponseType<CategoryDto>(StatusCodes.Status201Created)]
    public override Task<IActionResult> AddModel(CategoryDto modelDto)
    {
        return base.AddModel(modelDto);
    }

    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> UpdateEntry(int id, CategoryDto modelDto)
    {
        return base.UpdateEntry(id, modelDto);
    }

    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> PatchEntry(int id, JsonPatchDocument<Category> modelPatch)
    {
        return base.PatchEntry(id, modelPatch);
    }
}