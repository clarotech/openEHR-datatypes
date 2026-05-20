using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.DataTypes.TimeSpecification;

/// <summary>
/// Represents a general time specification, typically expressed as an HL7 GTS value.
/// openEHR RM 1.1.0 data_types.time_specification.DV_GENERAL_TIME_SPECIFICATION
/// </summary>
public sealed class DvGeneralTimeSpecification : DvTimeSpecification
{
    public DvGeneralTimeSpecification(DvParsable value) : base(value) { }
}
