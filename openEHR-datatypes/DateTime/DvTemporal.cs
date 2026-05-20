using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.DateTime;

/// <summary>
/// Abstract parent of date/time types. Represents an absolute quantity on the time axis.
/// openEHR RM 1.1.0 data_types.date_time.DV_TEMPORAL
/// </summary>
public abstract class DvTemporal : DvAbsoluteQuantity<DvDuration>
{
    public string Value { get; }

    protected DvTemporal(
        string value,
        DvDuration? accuracy = null,
        string? magnitudeStatus = null,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(accuracy, magnitudeStatus, normalRange, normalStatus, otherReferenceRanges)
    {
        Value = value;
    }

    public override string ToString() => Value;
}
