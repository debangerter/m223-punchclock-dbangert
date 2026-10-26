using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Model.Dto;

public class EntryDto : IValidatableObject
{
    // ReSharper disable once MemberCanBePrivate.Global Reason: Needed for JSON-Serialization
    public int Id { get; protected init; }
    
    [Required(ErrorMessage = "Missing Check-In")]
    public DateTime? CheckIn {  get; init; }
    
    [Required(ErrorMessage = "Missing Check-Out")]
    public DateTime? CheckOut { get; init; }

    public static EntryDto FromModel(Entry entry)
    {
        return new EntryDto
        {
            Id = entry.Id,
            CheckIn = entry.CheckIn,
            CheckOut = entry.CheckOut,
        };
    }
    
    public Entry ToModel()
    {
        return new Entry
        {
            Id = Id,
            CheckIn = CheckIn!.Value,
            CheckOut = CheckOut!.Value,
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn is null || CheckOut is null)  yield break;
    
        if (CheckIn.Value.Kind is not DateTimeKind.Utc || CheckOut.Value.Kind is not DateTimeKind.Utc)
            yield return new ValidationResult("Check-In and Check-Out must be of type UTC");
    
        if (CheckIn.Value > CheckOut.Value) 
            yield return new ValidationResult("Cannot check-out before checking in", [nameof(CheckIn), nameof(CheckOut)]);
    }
}