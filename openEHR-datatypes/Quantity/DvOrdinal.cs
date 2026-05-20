using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Models rankings and scores, e.g. pain intensity 0-10.
/// openEHR RM 1.1.0 data_types.quantity.DV_ORDINAL
/// </summary>
public class DvOrdinal : DvOrdered, IEquatable<DvOrdinal>, IComparable<DvOrdinal>
{
    public int Value { get; }
    public DvCodedText Symbol { get; }

    public DvOrdinal(
        int value,
        DvCodedText symbol,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(normalRange, normalStatus, otherReferenceRanges)
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        Value = value;
    }

    /// <summary>True if <paramref name="other"/> is a DvOrdinal (same comparable type).</summary>
    public override bool IsStrictlyComparableTo(DvOrdered other) => other is DvOrdinal;

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvOrdinal o) return Value.CompareTo(o.Value);
        throw new ArgumentException($"Cannot compare DvOrdinal with {other.GetType().Name}.");
    }

    public int CompareTo(DvOrdinal? other) => other is null ? 1 : Value.CompareTo(other.Value);

    public bool Equals(DvOrdinal? other) =>
        other is not null && Value == other.Value && Symbol.Equals(other.Symbol);

    public override bool Equals(object? obj) => obj is DvOrdinal o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(Value, Symbol);

    public static bool operator <(DvOrdinal left, DvOrdinal right) => left.CompareTo(right) < 0;
    public static bool operator >(DvOrdinal left, DvOrdinal right) => left.CompareTo(right) > 0;
    public static bool operator <=(DvOrdinal left, DvOrdinal right) => left.CompareTo(right) <= 0;
    public static bool operator >=(DvOrdinal left, DvOrdinal right) => left.CompareTo(right) >= 0;
}
