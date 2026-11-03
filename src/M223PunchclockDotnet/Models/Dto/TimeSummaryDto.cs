namespace M223PunchclockDotnet.Models.Dto;

public record TimeSummaryDto
{
    public DateOnly Date { get; init; }
    public TimeSpan Duration { get; init; }
}