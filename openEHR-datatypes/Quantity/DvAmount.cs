using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Abstract class defining the notion of a quantified amount.
/// openEHR RM 1.1.0 data_types.quantity.DV_AMOUNT
/// </summary>
public abstract class DvAmount : DvQuantified
{
    protected DvAmount(
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, accuracy, accuracyIsPercent, normalRange, normalStatus, otherReferenceRanges)
    {
    }

    /// <summary>
    /// Performs type-safe arithmetic addition. Must be overridden to return the correct concrete type.
    /// Throws if the types are incompatible (e.g. different units).
    /// </summary>
    public abstract DvAmount ArithmeticAdd(DvAmount other);

    /// <summary>Returns a negated copy of this amount.</summary>
    public abstract DvAmount Negate();

    public DvAmount Add(DvAmount other) => ArithmeticAdd(other);
    public DvAmount Subtract(DvAmount other) => ArithmeticAdd(other.Negate());
}
