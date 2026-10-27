using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Model;

public class Category : AbstractDbModel
{
    [Required] [StringLength(255)] public required string Title { get; set; }

    public ICollection<Entry> Entries { get; set; } = [];
}