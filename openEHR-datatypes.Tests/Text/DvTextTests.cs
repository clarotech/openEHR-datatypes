using FluentAssertions;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Text;

public class DvTextTests
{
    [Fact]
    public void Constructor_ValidValue_SetsValue()
    {
        var t = new DvText("Hello world");
        t.Value.Should().Be("Hello world");
        t.Formatting.Should().BeNull();
        t.Language.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_EmptyValue_Throws(string? value)
    {
        var act = () => new DvText(value!);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("plain")]
    [InlineData("plain_no_newlines")]
    [InlineData("markdown")]
    public void Constructor_ValidFormatting_Accepted(string formatting)
    {
        var t = new DvText("Text", formatting: formatting);
        t.Formatting.Should().Be(formatting);
    }

    [Fact]
    public void Constructor_InvalidFormatting_Throws()
    {
        var act = () => new DvText("Text", formatting: "html");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithMappings_SetsMappings()
    {
        var mappings = new[]
        {
            new TermMapping('=', new CodePhrase(new TerminologyId("ICD-10"), "E11"))
        };
        var t = new DvText("Diabetes", mappings: mappings);
        t.Mappings.Should().HaveCount(1);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        new DvText("Hello").Should().Be(new DvText("Hello"));
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        new DvText("Hello").Should().NotBe(new DvText("World"));
    }
}
