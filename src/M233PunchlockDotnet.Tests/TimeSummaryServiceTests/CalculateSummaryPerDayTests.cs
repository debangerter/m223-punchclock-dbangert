using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Models.Dto;
using M223PunchclockDotnet.Services;

namespace M233PunchlockDotnet.Tests.TimeSummaryServiceTests;

public class CalculateSummaryPerDayTests
{
    private readonly TimeSummaryService _timeSummaryService = new();


    [Fact]
    public void OneEntry_CheckoutIsBeforeCheckin_ThrowsArgumentException()
    {
        // Arrange
        ICollection<Entry> entries =
        [
            new()
            {
                CheckIn = DateTime.Parse("2026-11-02T16:30:00.0000Z").ToUniversalTime(),
                CheckOut = DateTime.Parse("2026-11-02T10:30:00.0000Z").ToUniversalTime()
            }
        ];

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = _timeSummaryService.CalculateSummaryPerDay(entries);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("entries", exception.ParamName);
    }

    [Fact]
    public void OneEntry_CheckoutIsOnNextDay_ReturnsUntilAndFromMidnight()
    {
        // Arrange
        ICollection<Entry> entries =
        [
            new()
            {
                CheckIn = DateTime.Parse("2026-11-02T16:30:00.0000Z").ToUniversalTime(),
                CheckOut = DateTime.Parse("2026-11-03T10:30:00.0000Z").ToUniversalTime()
            }
        ];

        ICollection<TimeSummaryDto> expected =
        [
            new() {Date = DateOnly.Parse("2026-11-02"), Duration = TimeSpan.Parse("07:30:00")},
            new() {Date = DateOnly.Parse("2026-11-03"), Duration = TimeSpan.Parse("10:30:00")}
        ];

        // Act
        ICollection<TimeSummaryDto> actual = _timeSummaryService.CalculateSummaryPerDay(entries);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(MultipleEntries))]
    public void MultipleEntries_ReturnsCorrectTimes(ICollection<Entry> entries, ICollection<TimeSummaryDto> expected)
    {
        // Act
        ICollection<TimeSummaryDto> actual = _timeSummaryService.CalculateSummaryPerDay(entries);

        // Assert
        Assert.Equal(expected, actual);
    }

    public static TheoryData<ICollection<Entry>, ICollection<TimeSummaryDto>> MultipleEntries()
    {
        return new TheoryData<ICollection<Entry>, ICollection<TimeSummaryDto>>
        {
            {
                [
                    new Entry
                    {
                        CheckIn = DateTime.Parse("2026-11-02T08:32:00.0000Z").ToUniversalTime(),
                        CheckOut = DateTime.Parse("2026-11-02T12:01:00.0000Z").ToUniversalTime()
                    },
                    new Entry
                    {
                        CheckIn = DateTime.Parse("2026-11-03T13:00:00.0000Z").ToUniversalTime(),
                        CheckOut = DateTime.Parse("2026-11-03T17:45:00.0000Z").ToUniversalTime()
                    },
                    new Entry
                    {
                        CheckIn = DateTime.Parse("2026-04-01T10:00:00.0000Z").ToUniversalTime(),
                        CheckOut = DateTime.Parse("2026-04-01T16:15:00.0000Z").ToUniversalTime()
                    }
                ],
                [
                    new TimeSummaryDto {Date = DateOnly.Parse("2026-04-01"), Duration = TimeSpan.Parse("06:15:00")},
                    new TimeSummaryDto {Date = DateOnly.Parse("2026-11-02"), Duration = TimeSpan.Parse("03:29:00")},
                    new TimeSummaryDto {Date = DateOnly.Parse("2026-11-03"), Duration = TimeSpan.Parse("04:45:00")}
                ]
            }
        };
    }
}