using FluentAssertions;
using OpenEHR.RM.DataTypes.Encapsulated;
using OpenEHR.RM.DataTypes.TimeSpecification;

namespace OpenEHR.RM.DataTypes.Tests.TimeSpecification;

public class DvTimeSpecificationTests
{
    [Fact]
    public void DvPeriodicTimeSpecification_Constructor_SetsValue()
    {
        var parsable = new DvParsable("PIVL<TS>{period=1 d}", "HL7-PIVL");
        var spec = new DvPeriodicTimeSpecification(parsable);
        spec.Value.Should().Be(parsable);
    }

    [Fact]
    public void DvGeneralTimeSpecification_Constructor_SetsValue()
    {
        var parsable = new DvParsable("2026-01-01T09:00:00/P1D", "HL7-GTS");
        var spec = new DvGeneralTimeSpecification(parsable);
        spec.Value.Should().Be(parsable);
    }

    [Fact]
    public void DvPeriodicTimeSpecification_NullValue_Throws()
    {
        var act = () => new DvPeriodicTimeSpecification(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void DvGeneralTimeSpecification_NullValue_Throws()
    {
        var act = () => new DvGeneralTimeSpecification(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void BothTypes_AreSubtypesOfDvTimeSpecification()
    {
        var parsable = new DvParsable("x", "plain");
        new DvPeriodicTimeSpecification(parsable).Should().BeAssignableTo<DvTimeSpecification>();
        new DvGeneralTimeSpecification(parsable).Should().BeAssignableTo<DvTimeSpecification>();
    }
}
