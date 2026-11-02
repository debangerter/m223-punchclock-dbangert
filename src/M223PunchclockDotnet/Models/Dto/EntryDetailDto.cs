using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Dto;

public record EntryDetailDto : AbstractDto<EntryDetail>
{
    [JsonConstructor]
    public EntryDetailDto(int id, DateTime? checkIn, DateTime? checkOut, CategoryDto? categoryDto, ICollection<TagDto> tagDtos)
    {
        Id = id;
        CheckIn = checkIn;
        CheckOut = checkOut;
        Category = categoryDto;
        Tags = tagDtos;
    }
    
    public EntryDetailDto(EntryDetail model) : base(model)
    {
        Id = model.Id;
        CheckIn = model.CheckIn;
        CheckOut = model.CheckOut;
        Category = model.Category is null ? null : new CategoryDto(model.Category);
        Tags = [.. model.Tags.Select(tag => new TagDto(tag))];
    }
    
    [Required(ErrorMessage = "Missing Check-In")]
    public DateTime? CheckIn { get; }
    
    [Required(ErrorMessage = "Missing Check-Out")]
    public DateTime? CheckOut { get; }
    
    public CategoryDto? Category { get; }

    public ICollection<TagDto> Tags { get; } = [];
    
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn is null || CheckOut is null) yield break;

        if (CheckIn.Value.Kind is not DateTimeKind.Utc || CheckOut.Value.Kind is not DateTimeKind.Utc)
            yield return new ValidationResult("Check-In and Check-Out must be of type UTC");

        if (CheckIn.Value > CheckOut.Value)
            yield return new ValidationResult("Cannot check-out before checking in",
                [nameof(CheckIn), nameof(CheckOut)]);
    }

    public override EntryDetail ToModel()
    {
        return new EntryDetail
        {
            Id = Id,
            CheckIn = CheckIn!.Value,
            CheckOut = CheckOut!.Value,
            Category = Category?.ToModel(),
            Tags = [.. Tags.Select(tagDto => tagDto.ToModel())],
        };
    }
}