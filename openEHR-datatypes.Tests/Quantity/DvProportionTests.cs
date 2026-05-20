using FluentAssertions;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvProportionTests
{
    [Fact]
    public void Ratio_ValidArgs_SetsMagnitude()
    {
        var p = new DvProportion(3.0, 4.0, ProportionKind.Ratio);
        p.Numerator.Should().Be(3.0);
        p.Denominator.Should().Be(4.0);
        p.Magnitude.Should().BeApproximately(0.75, 0.001);
    }

    [Fact]
    public void Percent_MustHaveDenominator100()
    {
        var p = new DvProportion(50.0, 100.0, ProportionKind.Percent);
        p.Magnitude.Should().Be(0.5);
    }

    [Fact]
    public void Percent_WrongDenominator_Throws()
    {
        var act = () => new DvProportion(50.0, 50.0, ProportionKind.Percent);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Unitary_MustHaveDenominator1()
    {
        var p = new DvProportion(0.7, 1.0, ProportionKind.Unitary);
        p.Magnitude.Should().Be(0.7);
    }

    [Fact]
    public void Unitary_WrongDenominator_Throws()
    {
        var act = () => new DvProportion(0.7, 2.0, ProportionKind.Unitary);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UnitaryRange_ValidRange_Accepted()
    {
        var p = new DvProportion(0.5, 1.0, ProportionKind.UnitaryRange);
        p.Magnitude.Should().Be(0.5);
    }

    [Fact]
    public void UnitaryRange_OutOfRange_Throws()
    {
        var act = () => new DvProportion(1.5, 1.0, ProportionKind.UnitaryRange);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Fraction_MustBeIntegers()
    {
        var p = new DvProportion(3.0, 4.0, ProportionKind.Fraction);
        p.IsIntegral().Should().BeTrue();
    }

    [Fact]
    public void Fraction_NonIntegerNumerator_Throws()
    {
        var act = () => new DvProportion(3.5, 4.0, ProportionKind.Fraction);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ratio_ZeroDenominator_Throws()
    {
        var act = () => new DvProportion(1.0, 0.0, ProportionKind.Ratio);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Add_SameTypeAndDenominator_ReturnsSum()
    {
        var a = new DvProportion(1.0, 4.0, ProportionKind.Ratio);
        var b = new DvProportion(2.0, 4.0, ProportionKind.Ratio);
        var result = (DvProportion)a.Add(b);
        result.Numerator.Should().Be(3.0);
    }

    [Fact]
    public void Negate_NegatesNumerator()
    {
        var p = new DvProportion(3.0, 4.0, ProportionKind.Ratio);
        var neg = (DvProportion)p.Negate();
        neg.Numerator.Should().Be(-3.0);
    }

    [Fact]
    public void CompareTo_OrdersByMagnitude()
    {
        var small = new DvProportion(1.0, 4.0, ProportionKind.Ratio);
        var large = new DvProportion(3.0, 4.0, ProportionKind.Ratio);
        small.CompareTo(large).Should().BeNegative();
    }

    [Fact]
    public void ToString_Percent_IncludesPercentSymbol()
    {
        new DvProportion(75.0, 100.0, ProportionKind.Percent).ToString().Should().Contain("%");
    }
}
