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
            List<EntryDto> allEntries = await entryService.FindAll();
            return Ok(allEntries);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<EntryDto>(StatusCodes.Status201Created)]
        public async Task<ActionResult<EntryDto>> AddEntry([FromBody] EntryDto entry)
        {
            EntryDto newElement = await entryService.AddEntry(entry);
            return CreatedAtAction(nameof(Get), new { id = newElement.Id }, newElement);
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
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] EntryDto entry)
        {
            EntryDto? updatedEntry = await entryService.UpdateEntry(id, entry);

            return updatedEntry is null ? NotFound() : Ok(updatedEntry);
        }

        [HttpPatch("{id:int}")]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchEntry(int id, [FromBody] JsonPatchDocument<Entry> patchDocument)
        {
            EntryDto? updatedEntry = await entryService.UpdateEntry(id, patchDocument);

            return updatedEntry is null ? NotFound() : Ok(updatedEntry);
        }
    }
}