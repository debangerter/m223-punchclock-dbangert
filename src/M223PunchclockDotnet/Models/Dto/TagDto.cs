using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Dto;

public record TagDto : AbstractDto<Tag>
{
    [JsonConstructor]
    public TagDto(int id, string title, ICollection<int> entryIds)
    {
        Id = id;
        Title = title;
        EntryIds = entryIds;
    }

    public TagDto(Tag model) : base(model)
    {
        Id = model.Id;
        Title = model.Title;
        EntryIds = [.. model.Entries.Select(e => e.Id)];
    }

    [Required(ErrorMessage = "Missing Tag title")]
    public string? Title { get; }

    public ICollection<int> EntryIds { get; private set; } = [];


    public override Tag ToModel()
    {
        return new Tag
        {
            Id = Id,
            Title = Title!,
        };
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Title)) yield return new ValidationResult("Missing Title", [nameof(Title)]);
    }
}