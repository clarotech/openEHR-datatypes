using FluentAssertions;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Tests.Uri;

public class DvEhrUriTests
{
    [Fact]
    public void Constructor_ValidEhrScheme_Accepted()
    {
        var uri = new DvEhrUri("ehr://system.example.com/abc123/versions/1");
        uri.Scheme.Should().Be("ehr");
    }

    [Fact]
    public void Constructor_NonEhrScheme_Throws()
    {
        var act = () => new DvEhrUri("https://example.com");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void EhrId_ExtractsFirstPathSegment()
    {
        var uri = new DvEhrUri("ehr://system.example.com/abc123/versions/1");
        uri.EhrId.Should().Be("abc123");
    }

    [Fact]
    public void IsSubtypeOfDvUri()
    {
        new DvEhrUri("ehr://host/ehrId").Should().BeAssignableTo<DvUri>();
    }
}
