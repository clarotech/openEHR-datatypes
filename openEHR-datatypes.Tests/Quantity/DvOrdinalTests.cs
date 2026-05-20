using FluentAssertions;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvOrdinalTests
{
    private static DvCodedText Symbol(int v) =>
        new DvCodedText($"Level {v}", new CodePhrase(new TerminologyId("local"), $"level{v}"));

    [Fact]
    public void Constructor_ValidArgs_SetsProperties()
    {
        var o = new DvOrdinal(2, Symbol(2));
        o.Value.Should().Be(2);
        o.Symbol.Value.Should().Be("Level 2");
    }

    [Fact]
    public void Constructor_NullSymbol_Throws()
    {
        var act = () => new DvOrdinal(1, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CompareTo_LowerValue_ReturnsNegative()
    {
        new DvOrdinal(1, Symbol(1)).CompareTo(new DvOrdinal(2, Symbol(2))).Should().BeNegative();
    }

    [Fact]
    public void CompareTo_HigherValue_ReturnsPositive()
    {
        new DvOrdinal(3, Symbol(3)).CompareTo(new DvOrdinal(1, Symbol(1))).Should().BePositive();
    }

    [Fact]
    public void CompareTo_EqualValue_ReturnsZero()
    {
        new DvOrdinal(2, Symbol(2)).CompareTo(new DvOrdinal(2, Symbol(2))).Should().Be(0);
    }

    [Fact]
    public void Operators_OrderCorrectly()
    {
        var low = new DvOrdinal(1, Symbol(1));
        var high = new DvOrdinal(3, Symbol(3));
        (low < high).Should().BeTrue();
        (high > low).Should().BeTrue();
        var low2 = new DvOrdinal(1, Symbol(1));
        (low <= low2).Should().BeTrue();
        var high2 = new DvOrdinal(3, Symbol(3));
        (high >= high2).Should().BeTrue();
    }

    [Fact]
    public void IsSimple_NoRanges_ReturnsTrue()
    {
        new DvOrdinal(1, Symbol(1)).IsSimple().Should().BeTrue();
    }
}
