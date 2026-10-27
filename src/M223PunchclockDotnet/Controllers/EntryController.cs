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
    public override async Task<IActionResult> AddModel(EntryDto modelDto, params object?[]? args)
    {
        (IEnumerable<Tag> tags, string? invalidTagId) = await ValidateTagIdsAsync(modelDto);

        if (!string.IsNullOrWhiteSpace(invalidTagId)) return NotFound(invalidTagId);

        return await base.AddModel(modelDto, args);
    }

    protected override void OnBeforeAddModel(ref Entry model, params object?[]? args)
    {
        base.OnBeforeAddModel(ref model, args);

        if (args is IEnumerable<Tag> tags) model.Tags = [.. tags];
    }


    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override async Task<IActionResult> UpdateEntry(int id, EntryDto modelDto, params object?[]? args)
    {
        (IEnumerable<Tag> tags, string? invalidTagId) = await ValidateTagIdsAsync(modelDto);
        
        if (!string.IsNullOrWhiteSpace(invalidTagId)) return NotFound(invalidTagId);
        
        return await base.UpdateEntry(id, modelDto, args);
    }

    protected override void OnBeforeUpdateModel(ref Entry model, params object?[]? args)
    {
        base.OnBeforeUpdateModel(ref model, args);

        if (args is IEnumerable<Tag> tags) model.Tags = tags;
    }

    [ProducesResponseType<EntryDto>(StatusCodes.Status200OK)]
    public override Task<IActionResult> PatchEntry(int id, JsonPatchDocument<Entry> modelPatch)
    {
        return base.PatchEntry(id, modelPatch);
    }

    private async Task<(IEnumerable<Tag> tags, string? invalidTagId)> ValidateTagIdsAsync(EntryDto modelDto)
    {
        List<Tag> tags = await tagService.FindAll();
        HashSet<int> tagIds = [.. tags.Select(tag => tag.Id)];

        foreach (int tagId in modelDto.TagIds)
        {
            if (!tagIds.Contains(tagId)) return ([], $"{nameof(Tag)}-ID {tagId}");
        }

        return (tags.Where(tag => modelDto.TagIds.Contains(tag.Id)), null);
    }
}