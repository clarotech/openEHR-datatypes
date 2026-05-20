using FluentAssertions;
using OpenEHR.RM.DataTypes.Basic;

namespace OpenEHR.RM.DataTypes.Tests.Basic;

public class DvBooleanTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_ValidValue_SetsValue(bool value)
    {
        var dv = new DvBoolean(value);
        dv.Value.Should().Be(value);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        new DvBoolean(true).Should().Be(new DvBoolean(true));
        new DvBoolean(false).Should().Be(new DvBoolean(false));
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        new DvBoolean(true).Should().NotBe(new DvBoolean(false));
    }

    [Fact]
    public void IsAssignableToDataValue()
    {
        var dv = new DvBoolean(true);
        dv.Should().BeAssignableTo<DataValue>();
    }

    [Fact]
    public void ToString_ReturnsStringRepresentation()
    {
        new DvBoolean(true).ToString().Should().Be("True");
        new DvBoolean(false).ToString().Should().Be("False");
    }
}
