using FluentAssertions;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.DataTypes.Tests.DateTime;

public class DvDateTimeTests
{
    [Theory]
    [InlineData("2026", 2026, null, null, null)]
    [InlineData("2026-05", 2026, 5, null, null)]
    [InlineData("2026-05-20", 2026, 5, 20, null)]
    [InlineData("2026-05-20T14:30:00", 2026, 5, 20, 14)]
    [InlineData("2026-05-20T14:30:00Z", 2026, 5, 20, 14)]
    [InlineData("2026-05-20T14:30:00+01:00", 2026, 5, 20, 14)]
    public void Constructor_ValidDateTimes_ParsesCorrectly(string value, int year, int? month, int? day, int? hour)
    {
        var dt = new DvDateTime(value);
        dt.Year.Should().Be(year);
        dt.Month.Should().Be(month);
        dt.Day.Should().Be(day);
        dt.Hour.Should().Be(hour);
    }

    [Theory]
    [InlineData("2026-13-01")]
    [InlineData("2026-05-20T25:00:00")]
    [InlineData("")]
    public void Constructor_InvalidDateTime_Throws(string value)
    {
        var act = () => new DvDateTime(value);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IsPartial_FullDateTime_ReturnsFalse()
    {
        new DvDateTime("2026-05-20T14:30:00").IsPartial.Should().BeFalse();
    }

    [Fact]
    public void IsPartial_YearOnly_ReturnsTrue()
    {
        new DvDateTime("2026").IsPartial.Should().BeTrue();
    }

    [Fact]
    public void HasTimezone_WithZ_ReturnsTrue()
    {
        new DvDateTime("2026-05-20T14:30:00Z").HasTimezone.Should().BeTrue();
    }

    [Fact]
    public void CompareTo_EarlierDateTime_ReturnsNegative()
    {
        var earlier = new DvDateTime("2020-01-01T00:00:00");
        var later = new DvDateTime("2026-01-01T00:00:00");
        earlier.CompareTo(later).Should().BeNegative();
    }

    [Fact]
    public void Diff_TwoDateTimes_ReturnsDuration()
    {
        var a = new DvDateTime("2026-01-01T00:00:00");
        var b = new DvDateTime("2026-01-02T00:00:00");
        var diff = a.Diff(b);
        diff.Should().BeOfType<DvDuration>();
    }
}
