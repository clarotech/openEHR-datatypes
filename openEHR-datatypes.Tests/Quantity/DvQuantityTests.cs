using FluentAssertions;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvQuantityTests
{
    [Fact]
    public void Constructor_ValidArgs_SetsProperties()
    {
        var q = new DvQuantity(98.6, "Cel", precision: 1);
        q.Magnitude.Should().Be(98.6);
        q.Units.Should().Be("Cel");
        q.Precision.Should().Be(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_EmptyUnits_Throws(string? units)
    {
        var act = () => new DvQuantity(1.0, units!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NegativePrecision_Throws()
    {
        var act = () => new DvQuantity(1.0, "kg", precision: -1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Add_SameUnits_ReturnsSummedQuantity()
    {
        var a = new DvQuantity(1.5, "kg");
        var b = new DvQuantity(2.5, "kg");
        var result = (DvQuantity)a.Add(b);
        result.Magnitude.Should().Be(4.0);
        result.Units.Should().Be("kg");
    }

    [Fact]
    public void Add_DifferentUnits_Throws()
    {
        var a = new DvQuantity(1.0, "kg");
        var b = new DvQuantity(1.0, "lb");
        var act = () => a.Add(b);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Subtract_SameUnits_ReturnsDifference()
    {
        var a = new DvQuantity(5.0, "kg");
        var b = new DvQuantity(2.0, "kg");
        var result = (DvQuantity)a.Subtract(b);
        result.Magnitude.Should().Be(3.0);
    }

    [Fact]
    public void Negate_ReturnsNegatedMagnitude()
    {
        var q = new DvQuantity(5.0, "kg");
        var neg = (DvQuantity)q.Negate();
        neg.Magnitude.Should().Be(-5.0);
        neg.Units.Should().Be("kg");
    }

    [Fact]
    public void CompareTo_SameUnits_OrdersCorrectly()
    {
        var small = new DvQuantity(1.0, "kg");
        var large = new DvQuantity(5.0, "kg");
        (small < large).Should().BeTrue();
        (large > small).Should().BeTrue();
    }

    [Fact]
    public void CompareTo_DifferentUnits_Throws()
    {
        var a = new DvQuantity(1.0, "kg");
        var b = new DvQuantity(1.0, "lb");
        var act = () => a.CompareTo(b);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void IsIntegral_WholeMagnitude_ReturnsTrue()
    {
        new DvQuantity(5.0, "mg").IsIntegral().Should().BeTrue();
    }

    [Fact]
    public void IsIntegral_FractionalMagnitude_ReturnsFalse()
    {
        new DvQuantity(5.5, "mg").IsIntegral().Should().BeFalse();
    }

    [Fact]
    public void MagnitudeStatus_InvalidValue_Throws()
    {
        var act = () => new DvQuantity(1.0, "kg", magnitudeStatus: "invalid");
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("=")]
    [InlineData("<")]
    [InlineData(">")]
    [InlineData("<=")]
    [InlineData(">=")]
    [InlineData("~")]
    public void MagnitudeStatus_ValidValues_Accepted(string status)
    {
        var q = new DvQuantity(1.0, "kg", magnitudeStatus: status);
        q.MagnitudeStatus.Should().Be(status);
    }

    [Fact]
    public void InheritsFromDvOrderedHierarchy()
    {
        var q = new DvQuantity(1.0, "kg");
        q.Should().BeAssignableTo<DvAmount>();
        q.Should().BeAssignableTo<DvQuantified>();
        q.Should().BeAssignableTo<DvOrdered>();
    }
}
