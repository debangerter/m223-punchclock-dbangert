using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers;

[Route("entries")]
public sealed class EntryController(IService<Entry> service, IService<Tag> tagService)
    : AbstractController<EntryDto, Entry>(service)
{
    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> GetModels()
    {
        return base.GetModels();
    }

    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<EntryDto>(StatusCodes.Status201Created)]
    public override async Task<IActionResult> AddModel(EntryDto modelDto)
    {
        string? invalidTagId = await ValidateTagIdsAsync(modelDto);

        if (!string.IsNullOrWhiteSpace(invalidTagId)) return NotFound(invalidTagId);

        return await base.AddModel(modelDto);
    }

    protected override async Task OnBeforeAddModelAsync(EntryDto modelDto, Entry model)
    {
        await base.OnBeforeAddModelAsync(modelDto, model);

        model.Tags = [.. await GetTagsAsync(modelDto)];
    }


    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override async Task<IActionResult> UpdateEntry(int id, EntryDto modelDto)
    {
        string? invalidTagId = await ValidateTagIdsAsync(modelDto);

        if (!string.IsNullOrWhiteSpace(invalidTagId)) return NotFound(invalidTagId);

        return await base.UpdateEntry(id, modelDto);
    }

    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> PatchEntry(int id, JsonPatchDocument<Entry> modelPatch)
    {
        return base.PatchEntry(id, modelPatch);
    }

    private async Task<string?> ValidateTagIdsAsync(EntryDto modelDto)
    {
        List<Tag> tags = await tagService.FindAll();
        HashSet<int> tagIds = [.. tags.Select(tag => tag.Id)];

        foreach (int tagId in modelDto.TagIds)
        {
            if (!tagIds.Contains(tagId))
                return $"{nameof(Tag)}-ID {tagId}";
        }

        return null;
    }

    private async Task<IEnumerable<Tag>> GetTagsAsync(EntryDto modelDto)
    {
        List<Tag> tags = await tagService.FindAll();

        return tags.Where(tag => modelDto.TagIds.Contains(tag.Id));
    }
}