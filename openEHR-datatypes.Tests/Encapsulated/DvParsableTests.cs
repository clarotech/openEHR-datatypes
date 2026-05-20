using FluentAssertions;
using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.DataTypes.Tests.Encapsulated;

public class DvParsableTests
{
    [Fact]
    public void Constructor_ValidArgs_SetsProperties()
    {
        var p = new DvParsable("<observation/>", "xml");
        p.Value.Should().Be("<observation/>");
        p.Formalism.Should().Be("xml");
    }

    [Theory]
    [InlineData(null, "xml")]
    [InlineData("", "xml")]
    [InlineData("value", null)]
    [InlineData("value", "")]
    public void Constructor_EmptyRequiredArgs_Throws(string? value, string? formalism)
    {
        var act = () => new DvParsable(value!, formalism!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Size_ReturnsUtf8ByteCount()
    {
        var p = new DvParsable("Hello", "plain");
        p.Size.Should().Be(5);
    }

    [Fact]
    public void Size_UnicodeContent_ReturnsCorrectByteCount()
    {
        var p = new DvParsable("£", "plain");
        p.Size.Should().Be(2); // £ is 2 bytes in UTF-8
    }

    [Fact]
    public void IsSubtypeOfDvEncapsulated()
    {
        new DvParsable("x", "plain").Should().BeAssignableTo<DvEncapsulated>();
    }
}
