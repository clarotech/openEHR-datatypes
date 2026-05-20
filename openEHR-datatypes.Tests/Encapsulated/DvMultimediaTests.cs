using System.Security.Cryptography;
using FluentAssertions;
using OpenEHR.RM.DataTypes.Encapsulated;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Tests.Encapsulated;

public class DvMultimediaTests
{
    private static CodePhrase MediaType(string type = "image/png") =>
        new CodePhrase(new TerminologyId("IANA"), type);

    [Fact]
    public void Constructor_WithData_SetsProperties()
    {
        var data = new byte[] { 1, 2, 3 };
        var mm = new DvMultimedia(MediaType(), data: data);
        mm.MediaType.CodeString.Should().Be("image/png");
        mm.Data.Should().BeEquivalentTo(data);
        mm.Size.Should().Be(3);
    }

    [Fact]
    public void Constructor_WithUri_SetsUri()
    {
        var uri = new DvUri("https://example.com/image.png");
        var mm = new DvMultimedia(MediaType(), uri: uri);
        mm.Uri.Should().Be(uri);
        mm.Size.Should().Be(0);
    }

    [Fact]
    public void Constructor_NeitherDataNorUri_Throws()
    {
        var act = () => new DvMultimedia(MediaType());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NullMediaType_Throws()
    {
        var act = () => new DvMultimedia(null!, data: new byte[] { 1 });
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IntegrityChecksPassed_ValidSha256_ReturnsTrue()
    {
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var hash = SHA256.HashData(data);
        var algo = new CodePhrase(new TerminologyId("openehr"), "SHA-256");
        var mm = new DvMultimedia(MediaType(), data: data,
            integrityCheckAlgorithm: algo, integrityCheck: hash);
        mm.IntegrityChecksPassed().Should().BeTrue();
    }

    [Fact]
    public void IntegrityChecksPassed_WrongHash_ReturnsFalse()
    {
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var wrongHash = new byte[32];
        var algo = new CodePhrase(new TerminologyId("openehr"), "SHA-256");
        var mm = new DvMultimedia(MediaType(), data: data,
            integrityCheckAlgorithm: algo, integrityCheck: wrongHash);
        mm.IntegrityChecksPassed().Should().BeFalse();
    }

    [Fact]
    public void IntegrityChecksPassed_NoData_ReturnsNull()
    {
        var uri = new DvUri("https://example.com/img.png");
        var mm = new DvMultimedia(MediaType(), uri: uri);
        mm.IntegrityChecksPassed().Should().BeNull();
    }

    [Fact]
    public void AlternateText_IsStoredCorrectly()
    {
        var mm = new DvMultimedia(MediaType(), data: new byte[] { 1 }, alternateText: "A photo");
        mm.AlternateText.Should().Be("A photo");
    }
}
