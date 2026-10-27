using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Model;

public abstract class AbstractDbModel
{
    [Key] public int Id { get; set; }
}