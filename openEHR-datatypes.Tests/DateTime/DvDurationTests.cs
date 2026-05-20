using FluentAssertions;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.DataTypes.Tests.DateTime;

public class DvDurationTests
{
    [Theory]
    [InlineData("P1Y", 1, 0, 0, 0, 0, 0, 0, false)]
    [InlineData("P2Y3M", 2, 3, 0, 0, 0, 0, 0, false)]
    [InlineData("P1W", 0, 0, 1, 0, 0, 0, 0, false)]
    [InlineData("P1DT2H3M4S", 0, 0, 0, 1, 2, 3, 4, false)]
    [InlineData("-P1Y", 1, 0, 0, 0, 0, 0, 0, true)]
    public void Constructor_ValidIso8601_ParsesComponents(
        string value, int y, int mo, int w, int d, int h, int m, double s, bool neg)
    {
        var dur = new DvDuration(value);
        dur.Years.Should().Be(y);
        dur.Months.Should().Be(mo);
        dur.Weeks.Should().Be(w);
        dur.Days.Should().Be(d);
        dur.Hours.Should().Be(h);
        dur.Minutes.Should().Be(m);
        dur.Seconds.Should().Be(s);
        dur.IsNegative.Should().Be(neg);
    }

    [Theory]
    [InlineData("1Y")]
    [InlineData("P")]
    [InlineData("invalid")]
    [InlineData("")]
    public void Constructor_InvalidValue_Throws(string value)
    {
        var act = () => new DvDuration(value);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FromComponents_BuildsValidIso8601()
    {
        var d = DvDuration.FromComponents(years: 1, months: 6, days: 15);
        d.Years.Should().Be(1);
        d.Months.Should().Be(6);
        d.Days.Should().Be(15);
    }

    [Fact]
    public void Negate_PositiveDuration_BecomesNegative()
    {
        var d = new DvDuration("P1Y");
        var neg = (DvDuration)d.Negate();
        neg.IsNegative.Should().BeTrue();
    }

    [Fact]
    public void Negate_NegativeDuration_BecomesPositive()
    {
        var d = new DvDuration("-P1Y");
        var pos = (DvDuration)d.Negate();
        pos.IsNegative.Should().BeFalse();
    }

    [Fact]
    public void Magnitude_NegativeDuration_IsNegative()
    {
        new DvDuration("-P1D").Magnitude.Should().BeNegative();
    }

    [Fact]
    public void Magnitude_PositiveDuration_IsPositive()
    {
        new DvDuration("P1D").Magnitude.Should().BePositive();
    }

    [Fact]
    public void CompareTo_OrdersByMagnitude()
    {
        var small = new DvDuration("P1D");
        var large = new DvDuration("P7D");
        small.CompareTo(large).Should().BeNegative();
    }
}
