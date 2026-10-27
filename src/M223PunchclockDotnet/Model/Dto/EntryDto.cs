using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Model.Dto;

public record EntryDto : AbstractDto<Entry>
{
    [JsonConstructor]
    public EntryDto(int id, DateTime? checkIn, DateTime? checkOut, int? categoryId, Category? category)
    {
        Id = id;
        CheckIn = checkIn;
        CheckOut = checkOut;
        CategoryId = categoryId;
        Category = category;
    }

    public EntryDto(Entry model) : base(model)
    {
        Id = model.Id;
        CheckIn = model.CheckIn;
        CheckOut = model.CheckOut;
        CategoryId = model.CategoryId;
        Category = model.Category;
    }

    [Required(ErrorMessage = "Missing Check-In")]
    public DateTime? CheckIn { get; }

    [Required(ErrorMessage = "Missing Check-Out")]
    public DateTime? CheckOut { get; }
    
    public int? CategoryId { get; }
    
    public Category? Category { get; }

    public override Entry ToModel()
    {
        return new Entry
        {
            Id = Id,
            CheckIn = CheckIn!.Value,
            CheckOut = CheckOut!.Value
        };
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn is null || CheckOut is null) yield break;

        if (CheckIn.Value.Kind is not DateTimeKind.Utc || CheckOut.Value.Kind is not DateTimeKind.Utc)
            yield return new ValidationResult("Check-In and Check-Out must be of type UTC");

        if (CheckIn.Value > CheckOut.Value)
            yield return new ValidationResult("Cannot check-out before checking in",
                [nameof(CheckIn), nameof(CheckOut)]);
    }
}