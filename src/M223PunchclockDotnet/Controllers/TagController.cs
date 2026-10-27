using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[Route("tags")]
public sealed class TagController(IService<Tag> service) 
    : AbstractController<TagDto, Tag>(service)
{
    [ProducesResponseType<TagDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> GetModels()
    {
        return base.GetModels();
    }

    [ProducesResponseType<TagDto>(StatusCodes.Status201Created)]
    public override Task<IActionResult> AddModel(TagDto modelDto)
    {
        return base.AddModel(modelDto);
    }

    [ProducesResponseType<TagDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> UpdateEntry(int id, TagDto modelDto)
    {
        return base.UpdateEntry(id, modelDto);
    }

    [ProducesResponseType<TagDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> PatchEntry(int id, JsonPatchDocument<Tag> modelPatch)
    {
        return base.PatchEntry(id, modelPatch);
    }
}