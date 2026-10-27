using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Model.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[Route("entries")]
public class EntryController(EntryService entryService)
    : AbstractController<EntryDto, EntryService, Entry>(entryService)
{
    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> GetModel()
    {
        return base.GetModel();
    }

    [ProducesResponseType<EntryDto>(StatusCodes.Status201Created)]
    public override Task<ActionResult<EntryDto>> AddModel(EntryDto modelDto)
    {
        return base.AddModel(modelDto);
    }

    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> UpdateEntry(int id, EntryDto modelDto)
    {
        return base.UpdateEntry(id, modelDto);
    }

    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> PatchEntry(int id, JsonPatchDocument<Entry> modelPatch)
    {
        return base.PatchEntry(id, modelPatch);
    }
}