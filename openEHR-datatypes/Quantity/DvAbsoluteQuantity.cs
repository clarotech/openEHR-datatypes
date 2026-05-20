using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Abstract class representing absolute quantities, i.e. quantities whose values are absolute
/// on some scale (with a defined zero point). Supports arithmetic with an accuracy type T.
/// openEHR RM 1.1.0 data_types.quantity.DV_ABSOLUTE_QUANTITY
/// </summary>
public abstract class DvAbsoluteQuantity<T> : DvQuantified where T : DvAmount
{
    /// <summary>Optional accuracy of this quantity expressed in the same units as T.</summary>
    public new T? Accuracy { get; }

    protected DvAbsoluteQuantity(
        T? accuracy = null,
        string? magnitudeStatus = null,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, null, false, normalRange, normalStatus, otherReferenceRanges)
    {
        Accuracy = accuracy;
    }

    /// <summary>Adds a delta amount to produce a new absolute quantity of the same type.</summary>
    public abstract DvAbsoluteQuantity<T> AddDelta(T delta);

    /// <summary>Returns the difference between this and another absolute quantity.</summary>
    public abstract T Diff(DvAbsoluteQuantity<T> other);
}
