using System.Net.Mime;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Model.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("entries")]
    public class EntryController(EntryService entryService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            List<Entry> entries = await entryService.FindAll();
            return Ok(entries.Select(entry => new EntryDto(entry)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<EntryDto>(StatusCodes.Status201Created)]
        public async Task<ActionResult<EntryDto>> AddEntry([FromBody] EntryDto entryDto)
        {
            Entry newEntry = await entryService.AddEntry(entryDto.ToModel());
            EntryDto newEntryDto = new(newEntry);
            return CreatedAtAction(nameof(Get), new { id = newEntryDto.Id }, newEntryDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var successful = await entryService.DeleteEntry(id);
            return successful ? NoContent() : NotFound();
        }

        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] EntryDto entryDto)
        {
            Entry? updatedEntry = await entryService.UpdateEntry(id, entryDto.ToModel());

            return updatedEntry is null ? NotFound() : Ok(new EntryDto(updatedEntry));
        }

        [HttpPatch("{id:int}")]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchEntry(int id, [FromBody] JsonPatchDocument<Entry> entryPatch)
        {
            Entry? updatedEntry = await entryService.UpdateEntry(id, entryPatch);

            return updatedEntry is null ? NotFound() : Ok(new EntryDto(updatedEntry));
        }
    }
}