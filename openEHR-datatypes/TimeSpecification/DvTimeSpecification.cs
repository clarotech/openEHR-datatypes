using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.DataTypes.TimeSpecification;

/// <summary>
/// Specifies points in time, possibly linked to the calendar, or a real world repeating event,
/// in a formal notation.
/// openEHR RM 1.1.0 data_types.time_specification.DV_TIME_SPECIFICATION
/// </summary>
public abstract class DvTimeSpecification : DataValue
{
    public DvParsable Value { get; }

    protected DvTimeSpecification(DvParsable value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
