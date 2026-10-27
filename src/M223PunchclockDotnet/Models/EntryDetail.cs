using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Models;

public class EntryDetail : AbstractDbModel
{
    [Required] public DateTime CheckIn { get; set; }

    [Required] public DateTime CheckOut { get; set; }

    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];
    
}