using FluentAssertions;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Text;

public class CodePhraseTests
{
    private static TerminologyId Snomed() => new("SNOMED-CT");

    [Fact]
    public void Constructor_ValidArgs_SetsProperties()
    {
        var cp = new CodePhrase(Snomed(), "73211009", "Diabetes mellitus");
        cp.TerminologyId.Value.Should().Be("SNOMED-CT");
        cp.CodeString.Should().Be("73211009");
        cp.PreferredTerm.Should().Be("Diabetes mellitus");
    }

    [Fact]
    public void Constructor_NoPreferredTerm_IsNull()
    {
        var cp = new CodePhrase(Snomed(), "73211009");
        cp.PreferredTerm.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_EmptyCodeString_Throws(string? code)
    {
        var act = () => new CodePhrase(Snomed(), code!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NullTerminologyId_Throws()
    {
        var act = () => new CodePhrase(null!, "123");
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_SameTerminologyAndCode_ReturnsTrue()
    {
        var a = new CodePhrase(Snomed(), "123");
        var b = new CodePhrase(Snomed(), "123");
        a.Should().Be(b);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        new CodePhrase(Snomed(), "111").Should().NotBe(new CodePhrase(Snomed(), "222"));
    }

    [Fact]
    public void ToString_ReturnsScopedFormat()
    {
        new CodePhrase(Snomed(), "73211009").ToString().Should().Be("SNOMED-CT::73211009");
    }
}
