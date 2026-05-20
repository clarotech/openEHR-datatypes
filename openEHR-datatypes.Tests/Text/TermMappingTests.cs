using FluentAssertions;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Text;

public class TermMappingTests
{
    private static CodePhrase Target() =>
        new CodePhrase(new TerminologyId("ICD-10"), "E11");

    [Theory]
    [InlineData('>', true, false, false, false)]
    [InlineData('=', false, true, false, false)]
    [InlineData('<', false, false, true, false)]
    [InlineData('?', false, false, false, true)]
    public void MatchMethods_ReturnCorrectValues(char match, bool broader, bool equivalent, bool narrower, bool unknown)
    {
        var tm = new TermMapping(match, Target());
        tm.Broader().Should().Be(broader);
        tm.Equivalent().Should().Be(equivalent);
        tm.Narrower().Should().Be(narrower);
        tm.Unknown().Should().Be(unknown);
    }

    [Fact]
    public void Constructor_InvalidMatch_Throws()
    {
        var act = () => new TermMapping('X', Target());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NullTarget_Throws()
    {
        var act = () => new TermMapping('=', null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_OptionalPurpose_SetsProperty()
    {
        var purpose = new DvCodedText("Translation", new CodePhrase(new TerminologyId("openehr"), "991"));
        var tm = new TermMapping('=', Target(), purpose);
        tm.Purpose.Should().Be(purpose);
    }
}
