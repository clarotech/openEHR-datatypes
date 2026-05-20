using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Abstract class defining the concept of ordered values which are not also quantities.
/// openEHR RM 1.1.0 data_types.quantity.DV_ORDERED
/// </summary>
public abstract class DvOrdered : DataValue, IComparable<DvOrdered>
{
    public ReferenceRange<DvOrdered>? NormalRange { get; }
    public CodePhrase? NormalStatus { get; }
    public IReadOnlyList<ReferenceRange<DvOrdered>>? OtherReferenceRanges { get; }

    protected DvOrdered(
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
    {
        NormalRange = normalRange;
        NormalStatus = normalStatus;
        OtherReferenceRanges = otherReferenceRanges;
    }

    public abstract int CompareTo(DvOrdered? other);

    /// <summary>
    /// True if this value and <paramref name="other"/> are of the same type (and, where
    /// applicable, have compatible units/scale) so that they can be meaningfully compared.
    /// openEHR RM 1.1.0: DV_ORDERED.is_strictly_comparable_to
    /// </summary>
    public abstract bool IsStrictlyComparableTo(DvOrdered other);

    /// <summary>
    /// True if there is no normal range or status and no other reference ranges.
    /// </summary>
    public bool IsSimple() => NormalRange is null && NormalStatus is null &&
                               (OtherReferenceRanges is null || OtherReferenceRanges.Count == 0);

    /// <summary>
    /// True if this value is in the normal range (if one is defined).
    /// Returns null if no normal range is set.
    /// </summary>
    public bool? IsNormal()
    {
        if (NormalRange is not null)
            return NormalRange.IsInRange(this);
        if (NormalStatus is not null)
            return NormalStatus.CodeString == "N";
        return null;
    }
}
