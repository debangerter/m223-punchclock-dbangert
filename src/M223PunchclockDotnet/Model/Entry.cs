using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Model;

public class Entry : AbstractDbModel
{
    [Required] public DateTime CheckIn { get; set; }

    [Required] public DateTime CheckOut { get; set; }

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}