using FluentAssertions;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvCountTests
{
    [Fact]
    public void Constructor_ValidMagnitude_SetsProperties()
    {
        var c = new DvCount(42);
        c.IntegerMagnitude.Should().Be(42);
        c.Magnitude.Should().Be(42.0);
    }

    [Fact]
    public void Add_TwoCounts_ReturnsSum()
    {
        var result = (DvCount)new DvCount(10).Add(new DvCount(5));
        result.IntegerMagnitude.Should().Be(15);
    }

    [Fact]
    public void Subtract_TwoCounts_ReturnsDifference()
    {
        var result = (DvCount)new DvCount(10).Subtract(new DvCount(4));
        result.IntegerMagnitude.Should().Be(6);
    }

    [Fact]
    public void Negate_PositiveCount_ReturnsNegative()
    {
        var result = (DvCount)new DvCount(5).Negate();
        result.IntegerMagnitude.Should().Be(-5);
    }

    [Fact]
    public void Add_NonDvCount_Throws()
    {
        var act = () => new DvCount(1).Add(new DvQuantity(1.0, "kg"));
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CompareTo_OrdersCorrectly()
    {
        (new DvCount(3) < new DvCount(5)).Should().BeTrue();
        (new DvCount(7) > new DvCount(2)).Should().BeTrue();
    }

    [Fact]
    public void CompareTo_DifferentType_Throws()
    {
        var act = () => new DvCount(1).CompareTo(new DvQuantity(1.0, "kg"));
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void NegativeCount_IsValid()
    {
        new DvCount(-100).IntegerMagnitude.Should().Be(-100);
    }
}
