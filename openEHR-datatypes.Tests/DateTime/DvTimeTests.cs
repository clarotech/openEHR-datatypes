using FluentAssertions;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.DataTypes.Tests.DateTime;

public class DvTimeTests
{
    [Theory]
    [InlineData("14", 14, null, null)]
    [InlineData("14:30", 14, 30, null)]
    [InlineData("14:30:00", 14, 30, 0)]
    [InlineData("14:30:00.500", 14, 30, 0)]
    [InlineData("14:30:00Z", 14, 30, 0)]
    [InlineData("14:30:00+05:30", 14, 30, 0)]
    public void Constructor_ValidTime_ParsesCorrectly(string value, int hour, int? minute, int? second)
    {
        var t = new DvTime(value);
        t.Hour.Should().Be(hour);
        t.Minute.Should().Be(minute);
        t.Second.Should().Be(second);
    }

    [Theory]
    [InlineData("25:00")]
    [InlineData("14:60")]
    [InlineData("")]
    [InlineData("not-a-time")]
    public void Constructor_InvalidTime_Throws(string value)
    {
        var act = () => new DvTime(value);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void HasTimezone_WithZ_ReturnsTrue()
    {
        new DvTime("14:30:00Z").HasTimezone.Should().BeTrue();
    }

    [Fact]
    public void HasTimezone_WithoutTimezone_ReturnsFalse()
    {
        new DvTime("14:30:00").HasTimezone.Should().BeFalse();
    }

    [Fact]
    public void FractionalSecond_Parsed()
    {
        new DvTime("14:30:00.500").FractionalSecond.Should().BeApproximately(0.5, 0.001);
    }

    [Fact]
    public void IsPartial_HourOnly_ReturnsTrue()
    {
        new DvTime("14").IsPartial.Should().BeTrue();
    }

    [Fact]
    public void CompareTo_EarlierTime_ReturnsNegative()
    {
        new DvTime("08:00:00").CompareTo(new DvTime("12:00:00")).Should().BeNegative();
    }

    [Fact]
    public void Diff_TwoTimes_ReturnsDuration()
    {
        var start = new DvTime("10:00:00");
        var end = new DvTime("12:30:00");
        var diff = start.Diff(end);
        diff.Should().BeOfType<DvDuration>();
    }
}
