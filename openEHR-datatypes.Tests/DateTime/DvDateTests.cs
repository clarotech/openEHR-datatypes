using FluentAssertions;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.DataTypes.Tests.DateTime;

public class DvDateTests
{
    [Theory]
    [InlineData("2026", 2026, null, null)]
    [InlineData("2026-05", 2026, 5, null)]
    [InlineData("2026-05-20", 2026, 5, 20)]
    public void Constructor_VariousPartialDates_ParsesCorrectly(string value, int year, int? month, int? day)
    {
        var d = new DvDate(value);
        d.Year.Should().Be(year);
        d.Month.Should().Be(month);
        d.Day.Should().Be(day);
    }

    [Theory]
    [InlineData("20260520")]
    [InlineData("2026-13")]
    [InlineData("2026-00")]
    [InlineData("")]
    [InlineData("not-a-date")]
    public void Constructor_InvalidDate_Throws(string value)
    {
        var act = () => new DvDate(value);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IsPartial_FullDate_ReturnsFalse()
    {
        new DvDate("2026-05-20").IsPartial.Should().BeFalse();
    }

    [Fact]
    public void IsPartial_YearOnly_ReturnsTrue()
    {
        new DvDate("2026").IsPartial.Should().BeTrue();
    }

    [Fact]
    public void ToDateOnly_FullDate_Converts()
    {
        var d = new DvDate("2026-05-20").ToDateOnly();
        d.Year.Should().Be(2026);
        d.Month.Should().Be(5);
        d.Day.Should().Be(20);
    }

    [Fact]
    public void ToDateOnly_PartialDate_Throws()
    {
        var act = () => new DvDate("2026").ToDateOnly();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CompareTo_EarlierDate_ReturnsNegative()
    {
        new DvDate("2020-01-01").CompareTo(new DvDate("2025-01-01")).Should().BeNegative();
    }

    [Fact]
    public void AddDelta_Days_ProducesCorrectDate()
    {
        var result = (DvDate)new DvDate("2026-01-01").AddDelta(DvDuration.FromComponents(days: 10));
        result.Value.Should().Be("2026-01-11");
    }

    [Fact]
    public void InheritsFromDvTemporalHierarchy()
    {
        var d = new DvDate("2026-05-20");
        d.Should().BeAssignableTo<DvTemporal>();
    }
}
