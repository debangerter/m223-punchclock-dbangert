using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;

namespace M223PunchclockDotnet.Services;

public interface ITimeSummaryService
{
    ICollection<TimeSummaryDto> CalculateSummaryPerDay(ICollection<Entry> entries);
}