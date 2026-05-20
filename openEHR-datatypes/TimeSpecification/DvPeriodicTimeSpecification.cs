using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.DataTypes.TimeSpecification;

/// <summary>
/// Represents a periodic time specification, typically expressed as an HL7 PIVL_TS value.
/// openEHR RM 1.1.0 data_types.time_specification.DV_PERIODIC_TIME_SPECIFICATION
/// </summary>
public sealed class DvPeriodicTimeSpecification : DvTimeSpecification
{
    public DvPeriodicTimeSpecification(DvParsable value) : base(value) { }
}
