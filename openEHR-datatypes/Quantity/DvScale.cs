using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// A data type that represents scale values, where the values are real numbers (not just integers).
/// Extends DV_ORDINAL with real-number value support.
/// openEHR RM 1.1.0 data_types.quantity.DV_SCALE
/// </summary>
public sealed class DvScale : DvOrdered, IEquatable<DvScale>, IComparable<DvScale>
{
    public double Value { get; }
    public DvText Symbol { get; }

    public DvScale(
        double value,
        DvText symbol,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(normalRange, normalStatus, otherReferenceRanges)
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        Value = value;
    }

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvScale s) return Value.CompareTo(s.Value);
        throw new ArgumentException($"Cannot compare DvScale with {other.GetType().Name}.");
    }

    public int CompareTo(DvScale? other) => other is null ? 1 : Value.CompareTo(other.Value);

    public bool Equals(DvScale? other) =>
        other is not null && Value == other.Value && Symbol.Equals(other.Symbol);

    public override bool Equals(object? obj) => obj is DvScale s && Equals(s);
    public override int GetHashCode() => HashCode.Combine(Value, Symbol);

    public static bool operator <(DvScale left, DvScale right) => left.CompareTo(right) < 0;
    public static bool operator >(DvScale left, DvScale right) => left.CompareTo(right) > 0;
    public static bool operator <=(DvScale left, DvScale right) => left.CompareTo(right) <= 0;
    public static bool operator >=(DvScale left, DvScale right) => left.CompareTo(right) >= 0;
}
