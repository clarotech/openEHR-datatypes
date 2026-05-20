using FluentAssertions;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

/// <summary>
/// Tests for DV_QUANTIFIED functions defined in openEHR RM 1.1.0:
///   • magnitude_status_is_exact()
///   • valid_magnitude_status(s)
///   • is_strictly_comparable_to(other) — via every concrete subclass
/// </summary>
public class DvQuantifiedTests
{
    // -----------------------------------------------------------------------
    // valid_magnitude_status
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData(null,  true)]
    [InlineData("=",   true)]
    [InlineData("<",   true)]
    [InlineData(">",   true)]
    [InlineData("<=",  true)]
    [InlineData(">=",  true)]
    [InlineData("~",   true)]
    [InlineData("??",  false)]
    [InlineData("exact", false)]
    [InlineData("",    false)]
    public void ValidMagnitudeStatus_ReturnsExpected(string? s, bool expected)
    {
        DvQuantified.ValidMagnitudeStatus(s).Should().Be(expected);
    }

    // -----------------------------------------------------------------------
    // magnitude_status_is_exact
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData(null, true)]
    [InlineData("=",  true)]
    [InlineData("<",  false)]
    [InlineData(">",  false)]
    [InlineData("<=", false)]
    [InlineData(">=", false)]
    [InlineData("~",  false)]
    public void MagnitudeStatusIsExact_ReturnsExpected(string? status, bool expected)
    {
        var q = new DvQuantity(1.0, "kg", magnitudeStatus: status);
        q.MagnitudeStatusIsExact().Should().Be(expected);
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvQuantity (units must match)
    // -----------------------------------------------------------------------

    [Fact]
    public void DvQuantity_IsStrictlyComparableTo_SameUnits_ReturnsTrue()
    {
        var a = new DvQuantity(1.0, "kg");
        var b = new DvQuantity(2.0, "kg");
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvQuantity_IsStrictlyComparableTo_DifferentUnits_ReturnsFalse()
    {
        var a = new DvQuantity(1.0, "kg");
        var b = new DvQuantity(1.0, "lb");
        a.IsStrictlyComparableTo(b).Should().BeFalse();
    }

    [Fact]
    public void DvQuantity_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var q = new DvQuantity(1.0, "kg");
        var c = new DvCount(1);
        q.IsStrictlyComparableTo(c).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvCount
    // -----------------------------------------------------------------------

    [Fact]
    public void DvCount_IsStrictlyComparableTo_AnotherDvCount_ReturnsTrue()
    {
        var a = new DvCount(3);
        var b = new DvCount(7);
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvCount_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var c = new DvCount(1);
        var q = new DvQuantity(1.0, "mg");
        c.IsStrictlyComparableTo(q).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvProportion (type must match)
    // -----------------------------------------------------------------------

    [Fact]
    public void DvProportion_IsStrictlyComparableTo_SameType_ReturnsTrue()
    {
        var a = new DvProportion(25, 100, ProportionKind.Percent);
        var b = new DvProportion(50, 100, ProportionKind.Percent);
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvProportion_IsStrictlyComparableTo_DifferentKind_ReturnsFalse()
    {
        var a = new DvProportion(25, 100, ProportionKind.Percent);
        var b = new DvProportion(3, 4, ProportionKind.Ratio);
        a.IsStrictlyComparableTo(b).Should().BeFalse();
    }

    [Fact]
    public void DvProportion_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var p = new DvProportion(1, 4, ProportionKind.Ratio);
        var c = new DvCount(1);
        p.IsStrictlyComparableTo(c).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvOrdinal
    // -----------------------------------------------------------------------

    private static DvCodedText OrdinalSymbol(int v) =>
        new DvCodedText($"Level {v}", new CodePhrase(new TerminologyId("local"), $"level{v}"));

    [Fact]
    public void DvOrdinal_IsStrictlyComparableTo_AnotherDvOrdinal_ReturnsTrue()
    {
        var a = new DvOrdinal(1, OrdinalSymbol(1));
        var b = new DvOrdinal(2, OrdinalSymbol(2));
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvOrdinal_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var o = new DvOrdinal(1, OrdinalSymbol(1));
        var c = new DvCount(1);
        o.IsStrictlyComparableTo(c).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvScale
    // -----------------------------------------------------------------------

    [Fact]
    public void DvScale_IsStrictlyComparableTo_AnotherDvScale_ReturnsTrue()
    {
        var a = new DvScale(1.5, new DvText("mild"));
        var b = new DvScale(3.0, new DvText("moderate"));
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvScale_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var s = new DvScale(1.0, new DvText("mild"));
        var q = new DvQuantity(1.0, "kg");
        s.IsStrictlyComparableTo(q).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvDuration
    // -----------------------------------------------------------------------

    [Fact]
    public void DvDuration_IsStrictlyComparableTo_AnotherDvDuration_ReturnsTrue()
    {
        var a = new DvDuration("P1Y");
        var b = new DvDuration("P2Y");
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvDuration_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var d = new DvDuration("P1Y");
        var c = new DvCount(1);
        d.IsStrictlyComparableTo(c).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvDate
    // -----------------------------------------------------------------------

    [Fact]
    public void DvDate_IsStrictlyComparableTo_AnotherDvDate_ReturnsTrue()
    {
        var a = new DvDate("2024-01-01");
        var b = new DvDate("2025-06-15");
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvDate_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var d = new DvDate("2024-01-01");
        var dt = new DvDateTime("2024-01-01T12:00:00");
        d.IsStrictlyComparableTo(dt).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvTime
    // -----------------------------------------------------------------------

    [Fact]
    public void DvTime_IsStrictlyComparableTo_AnotherDvTime_ReturnsTrue()
    {
        var a = new DvTime("08:30:00");
        var b = new DvTime("14:00:00");
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvTime_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var t = new DvTime("08:30:00");
        var d = new DvDate("2024-01-01");
        t.IsStrictlyComparableTo(d).Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // is_strictly_comparable_to — DvDateTime
    // -----------------------------------------------------------------------

    [Fact]
    public void DvDateTime_IsStrictlyComparableTo_AnotherDvDateTime_ReturnsTrue()
    {
        var a = new DvDateTime("2024-01-01T08:00:00");
        var b = new DvDateTime("2024-06-01T12:30:00");
        a.IsStrictlyComparableTo(b).Should().BeTrue();
    }

    [Fact]
    public void DvDateTime_IsStrictlyComparableTo_DifferentType_ReturnsFalse()
    {
        var dt = new DvDateTime("2024-01-01T08:00:00");
        var d  = new DvDate("2024-01-01");
        dt.IsStrictlyComparableTo(d).Should().BeFalse();
    }
}
