using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[ApiController]
[Route("entry-details")]
public sealed class EntryDetailController(IService<EntryDetail> service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<EntryDetailDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModels()
    {
        List<EntryDetail> models = await service.FindAll();

        return Ok(models.Select(model => new EntryDetailDto(model)));
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<EntryDetailDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModel(int id)
    {
        EntryDetail? entryDetail = await service.FindById(id);

        return entryDetail is null ? NotFound() : Ok(new EntryDetailDto(entryDetail));
    }
}