using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Model.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[Route("categories")]
public class CategoryController(CategoryService categoryService) : AbstractController<CategoryDto, CategoryService, Category>(categoryService)
{
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> GetModel()
    {
        return base.GetModel();
    }

    [ProducesResponseType<CategoryDto>(StatusCodes.Status201Created)]
    public override Task<ActionResult<CategoryDto>> AddModel(CategoryDto modelDto)
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