using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;

namespace M223PunchclockDotnet.Services;

public class TimeSummaryService : ITimeSummaryService
{
    public ICollection<TimeSummaryDto> CalculateSummaryPerDay(ICollection<Entry> entries)
    {
        List<TimeSummaryDto> summaries = [];
        foreach (Entry entry in entries)
        {
            DateTime start = entry.CheckIn;
            DateTime end = entry.CheckOut;
            if (start.Kind != DateTimeKind.Utc || end.Kind != DateTimeKind.Utc || end < start)
                throw new ArgumentException("Entries require UTC timestamps in chronological order.", nameof(entries));

            if (start == end)
                AddDuration(summaries, DateOnly.FromDateTime(start), TimeSpan.Zero);

            while (start < end)
            {
                // Avoid computing a day beyond DateTime.MaxValue for the last segment.
                DateTime segmentEnd = start.Date == end.Date ? end : start.Date.AddDays(1);
                AddDuration(summaries, DateOnly.FromDateTime(start), segmentEnd - start);
                start = segmentEnd;
            }
        }

        return [.. summaries.OrderBy(summary => summary.Date)];
    }

    private static void AddDuration(List<TimeSummaryDto> summaries, DateOnly date, TimeSpan duration)
    {
        for (int i = 0; i < summaries.Count; i++)
        {
            if (summaries[i].Date != date) continue;
            summaries[i] = new TimeSummaryDto { Date = date, Duration = summaries[i].Duration + duration };
            return;
        }

        summaries.Add(new TimeSummaryDto { Date = date, Duration = duration });
    }
}