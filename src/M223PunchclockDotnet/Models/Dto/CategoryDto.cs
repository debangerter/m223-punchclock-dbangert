using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Dto;

public record CategoryDto : AbstractDto<Category>
{
    [JsonConstructor]
    public CategoryDto(int id, string title, ICollection<int> entryIds)
    {
        Id = id;
        Title = title;
        EntryIds = entryIds;
    }

    public CategoryDto(Category model) : base(model)
    {
        Id = model.Id;
        Title = model.Title;
        EntryIds = [.. model.Entries.Select(entry => entry.Id)];
    }

    [Required(ErrorMessage = "Missing Category title")]
    public string? Title { get; }

    public ICollection<int> EntryIds { get; } = [];

    public override Category ToModel()
    {
        return new Category
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