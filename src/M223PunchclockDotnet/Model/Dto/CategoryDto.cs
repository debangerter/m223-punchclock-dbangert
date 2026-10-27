using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Model.Dto;

public record CategoryDto : AbstractDto<Category>
{
    [JsonConstructor]
    public CategoryDto(int id, string title)
    {
        Id = id;
        Title = title;
    }
    
    public CategoryDto(Category model) : base(model)
    {
        Id = model.Id;
        Title = model.Title;
    }
    
    [Required(ErrorMessage = "Missing Category title")]
    public string? Title { get; }
    
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
        if (string.IsNullOrWhiteSpace(Title)) yield break;
    }
}