using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Dto;

public abstract record AbstractDto<TModel> : IValidatableObject
{
    [JsonConstructor]
    protected AbstractDto(params object?[]? props)
    {
    }

    protected AbstractDto(TModel model)
    {
    }

    public int Id { get; protected init; }

    public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

    public abstract TModel ToModel();
}