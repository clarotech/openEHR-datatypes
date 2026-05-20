using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Quantity;

public class ReferenceRangeTests
{
    private static DvCount MakeCount(long value) => new(value);

    [Fact]
    public void IsInRange_ValueWithinRange_ReturnsTrue()
    {
        var range = new ReferenceRange<DvOrdered>(
            new DvInterval<DvOrdered>(MakeCount(10), MakeCount(20)),
            new DvText("Normal"));
        range.IsInRange(MakeCount(15)).Should().BeTrue();
    }

    [Fact]
    public void IsInRange_ValueOutsideRange_ReturnsFalse()
    {
        var range = new ReferenceRange<DvOrdered>(
            new DvInterval<DvOrdered>(MakeCount(10), MakeCount(20)));
        range.IsInRange(MakeCount(25)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_NullRange_Throws()
    {
        var act = () => new ReferenceRange<DvOrdered>(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_OptionalMeaning_SetsProperty()
    {
        var meaning = new DvText("Normal");
        var rr = new ReferenceRange<DvOrdered>(
            new DvInterval<DvOrdered>(MakeCount(0), MakeCount(100)),
            meaning);
        rr.Meaning.Should().Be(meaning);
    }

    [Fact]
    public void IsInRange_AtLowerBound_ReturnsTrue()
    {
        var range = new ReferenceRange<DvOrdered>(
            new DvInterval<DvOrdered>(MakeCount(10), MakeCount(20)));
        range.IsInRange(MakeCount(10)).Should().BeTrue();
    }

    [Fact]
    public void IsInRange_AtUpperBound_ReturnsTrue()
    {
        var range = new ReferenceRange<DvOrdered>(
            new DvInterval<DvOrdered>(MakeCount(10), MakeCount(20)));
        range.IsInRange(MakeCount(20)).Should().BeTrue();
    }
}
