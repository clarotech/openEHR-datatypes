using FluentAssertions;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvScaleTests
{
    private static DvText Symbol(double v) => new DvText($"Score {v}");

    [Fact]
    public void Constructor_ValidRealValue_SetsProperties()
    {
        var s = new DvScale(0.5, Symbol(0.5));
        s.Value.Should().Be(0.5);
        s.Symbol.Value.Should().Be("Score 0.5");
    }

    [Fact]
    public void CompareTo_LowerValue_ReturnsNegative()
    {
        new DvScale(0.5, Symbol(0.5)).CompareTo(new DvScale(1.0, Symbol(1.0))).Should().BeNegative();
    }

    [Fact]
    public void RealValuesDifferentFromInteger()
    {
        var scale = new DvScale(2.5, Symbol(2.5));
        scale.Value.Should().NotBe(2);
        scale.Value.Should().Be(2.5);
    }

    [Fact]
    public void IsSubtypeOfDvOrdered()
    {
        new DvScale(1.0, Symbol(1.0)).Should().BeAssignableTo<DvOrdered>();
    }
}
