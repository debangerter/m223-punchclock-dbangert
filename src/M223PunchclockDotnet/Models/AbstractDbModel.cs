using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Models;

public abstract class AbstractDbModel
{
    [Key] public int Id { get; set; }
}