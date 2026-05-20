using FluentAssertions;
using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Basic;

public class DvStateTests
{
    private static DvCodedText MakeCodedText(string code = "active") =>
        new("Active", new CodePhrase(new TerminologyId("local"), code));

    [Fact]
    public void Constructor_ValidValue_SetsProperties()
    {
        var coded = MakeCodedText();
        var state = new DvState(coded, isTerminal: false);
        state.Value.Should().Be(coded);
        state.IsTerminal.Should().BeFalse();
    }

    [Fact]
    public void Constructor_TerminalState_SetsIsTerminal()
    {
        var state = new DvState(MakeCodedText("completed"), isTerminal: true);
        state.IsTerminal.Should().BeTrue();
    }

    [Fact]
    public void Constructor_NullValue_Throws()
    {
        var act = () => new DvState(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_SameCodeAndTerminal_ReturnsTrue()
    {
        var coded = MakeCodedText();
        new DvState(coded, false).Should().Be(new DvState(coded, false));
    }

    [Fact]
    public void Equals_DifferentIsTerminal_ReturnsFalse()
    {
        var coded = MakeCodedText();
        new DvState(coded, true).Should().NotBe(new DvState(coded, false));
    }
}
