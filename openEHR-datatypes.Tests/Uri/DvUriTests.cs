using FluentAssertions;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Tests.Uri;

public class DvUriTests
{
    [Theory]
    [InlineData("https://example.com/path")]
    [InlineData("http://example.com")]
    [InlineData("urn:oid:2.16.840.1.113883")]
    [InlineData("/relative/path")]
    public void Constructor_ValidUri_SetsValue(string uri)
    {
        var dv = new DvUri(uri);
        dv.Value.Should().Be(uri);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_EmptyValue_Throws(string? value)
    {
        var act = () => new DvUri(value!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Scheme_AbsoluteUri_ReturnsScheme()
    {
        new DvUri("https://example.com").Scheme.Should().Be("https");
    }

    [Fact]
    public void Path_AbsoluteUri_ReturnsPath()
    {
        new DvUri("https://example.com/foo/bar").Path.Should().Be("/foo/bar");
    }

    [Fact]
    public void Query_UriWithQuery_ReturnsQuery()
    {
        new DvUri("https://example.com/path?key=value").Query.Should().Be("key=value");
    }

    [Fact]
    public void Fragment_UriWithFragment_ReturnsFragment()
    {
        new DvUri("https://example.com/page#section").Fragment.Should().Be("section");
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        new DvUri("https://example.com").Should().Be(new DvUri("https://example.com"));
    }
}
