using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Model.Dto;

public abstract record AbstractDto<TModel> : IValidatableObject
{
    [JsonConstructor]
    protected AbstractDto(params object?[]? props)
    {
    }

    protected AbstractDto(TModel model)
    {
    }

    public abstract TModel ToModel();

    public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
}