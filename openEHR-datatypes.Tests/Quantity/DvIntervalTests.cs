using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class DvIntervalTests
{
    private static DvCount C(long v) => new(v);

    [Fact]
    public void Constructor_ValidBounds_SetsProperties()
    {
        var iv = new DvInterval<DvCount>(C(1), C(10));
        iv.Lower!.IntegerMagnitude.Should().Be(1);
        iv.Upper!.IntegerMagnitude.Should().Be(10);
        iv.LowerUnbounded.Should().BeFalse();
        iv.UpperUnbounded.Should().BeFalse();
        iv.LowerIncluded.Should().BeTrue();
        iv.UpperIncluded.Should().BeTrue();
    }

    [Fact]
    public void Constructor_LowerGreaterThanUpper_Throws()
    {
        var act = () => new DvInterval<DvCount>(C(10), C(1));
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_UnboundedWithNonNullValue_Throws()
    {
        var act = () => new DvInterval<DvCount>(C(1), null, lowerUnbounded: true);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(0, false)]
    [InlineData(11, false)]
    public void Has_InclusiveBounds_ReturnsExpected(int value, bool expected)
    {
        var iv = new DvInterval<DvCount>(C(1), C(10));
        iv.Has(C(value)).Should().Be(expected);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(9, true)]
    [InlineData(10, false)]
    public void Has_ExclusiveBounds_ReturnsExpected(int value, bool expected)
    {
        var iv = new DvInterval<DvCount>(C(1), C(10), lowerIncluded: false, upperIncluded: false);
        iv.Has(C(value)).Should().Be(expected);
    }

    [Fact]
    public void Has_UnboundedInterval_AlwaysTrue()
    {
        var iv = DvInterval<DvCount>.Unbounded();
        iv.Has(C(long.MinValue)).Should().BeTrue();
        iv.Has(C(0)).Should().BeTrue();
        iv.Has(C(long.MaxValue)).Should().BeTrue();
    }

    [Fact]
    public void Intersects_OverlappingIntervals_ReturnsTrue()
    {
        var a = new DvInterval<DvCount>(C(1), C(5));
        var b = new DvInterval<DvCount>(C(3), C(8));
        a.Intersects(b).Should().BeTrue();
        b.Intersects(a).Should().BeTrue();
    }

    [Fact]
    public void Intersects_NonOverlappingIntervals_ReturnsFalse()
    {
        var a = new DvInterval<DvCount>(C(1), C(4));
        var b = new DvInterval<DvCount>(C(5), C(10));
        a.Intersects(b).Should().BeFalse();
    }

    [Fact]
    public void Contains_SubInterval_ReturnsTrue()
    {
        var outer = new DvInterval<DvCount>(C(1), C(10));
        var inner = new DvInterval<DvCount>(C(3), C(7));
        outer.Contains(inner).Should().BeTrue();
    }

    [Fact]
    public void Contains_PartiallyOverlappingInterval_ReturnsFalse()
    {
        var a = new DvInterval<DvCount>(C(1), C(5));
        var b = new DvInterval<DvCount>(C(3), C(8));
        a.Contains(b).Should().BeFalse();
    }
}
