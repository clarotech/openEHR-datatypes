using FluentAssertions;
using OpenEHR.RM.DataTypes.Basic;

namespace OpenEHR.RM.DataTypes.Tests.Basic;

public class DvIdentifierTests
{
    [Fact]
    public void Constructor_RequiredId_SetsId()
    {
        var id = new DvIdentifier("12345");
        id.Id.Should().Be("12345");
        id.Issuer.Should().BeNull();
        id.Assigner.Should().BeNull();
        id.Type.Should().BeNull();
    }

    [Fact]
    public void Constructor_AllProperties_SetsAllProperties()
    {
        var id = new DvIdentifier("12345", "NHS", "GP Surgery", "NHSNumber");
        id.Id.Should().Be("12345");
        id.Issuer.Should().Be("NHS");
        id.Assigner.Should().Be("GP Surgery");
        id.Type.Should().Be("NHSNumber");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_EmptyOrNullId_Throws(string? id)
    {
        var act = () => new DvIdentifier(id!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = new DvIdentifier("123", "NHS");
        var b = new DvIdentifier("123", "NHS");
        a.Should().Be(b);
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        new DvIdentifier("123").Should().NotBe(new DvIdentifier("456"));
    }
}
