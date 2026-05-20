using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Quantified type representing a measured value with units.
/// openEHR RM 1.1.0 data_types.quantity.DV_QUANTITY
/// </summary>
public sealed class DvQuantity : DvAmount, IEquatable<DvQuantity>, IComparable<DvQuantity>
{
    private readonly double _magnitude;

    public override double Magnitude => _magnitude;
    public string Units { get; }
    public int? Precision { get; }
    public string? UnitsSystem { get; }
    public string? UnitsDisplayName { get; }

    public DvQuantity(
        double magnitude,
        string units,
        int? precision = null,
        string? unitsSystem = null,
        string? unitsDisplayName = null,
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, accuracy, accuracyIsPercent, normalRange, normalStatus, otherReferenceRanges)
    {
        if (string.IsNullOrEmpty(units))
            throw new ArgumentException("Units must not be null or empty.", nameof(units));
        if (precision is < 0)
            throw new ArgumentOutOfRangeException(nameof(precision), "Precision must not be negative.");

        _magnitude = magnitude;
        Units = units;
        Precision = precision;
        UnitsSystem = unitsSystem;
        UnitsDisplayName = unitsDisplayName;
    }

    /// <summary>True if the quantity represents an integral value (no fractional part).</summary>
    public bool IsIntegral() => Precision == 0 || _magnitude == Math.Truncate(_magnitude);

    public override DvAmount ArithmeticAdd(DvAmount other)
    {
        if (other is not DvQuantity q)
            throw new ArgumentException("Can only add DvQuantity to DvQuantity.", nameof(other));
        if (q.Units != Units)
            throw new InvalidOperationException($"Cannot add quantities with different units: '{Units}' vs '{q.Units}'.");
        return new DvQuantity(_magnitude + q._magnitude, Units, Precision, UnitsSystem, UnitsDisplayName);
    }

    public override DvAmount Negate() =>
        new DvQuantity(-_magnitude, Units, Precision, UnitsSystem, UnitsDisplayName, MagnitudeStatus, Accuracy, AccuracyIsPercent);

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvQuantity q)
        {
            if (q.Units != Units)
                throw new InvalidOperationException($"Cannot compare quantities with different units: '{Units}' vs '{q.Units}'.");
            return _magnitude.CompareTo(q._magnitude);
        }
        throw new ArgumentException($"Cannot compare DvQuantity with {other.GetType().Name}.");
    }

    public int CompareTo(DvQuantity? other) => CompareTo((DvOrdered?)other);

    public bool Equals(DvQuantity? other) =>
        other is not null && _magnitude == other._magnitude && Units == other.Units;

    public override bool Equals(object? obj) => obj is DvQuantity q && Equals(q);
    public override int GetHashCode() => HashCode.Combine(_magnitude, Units);

    public override string ToString() =>
        Precision.HasValue
            ? $"{_magnitude.ToString($"F{Precision}")} {Units}"
            : $"{_magnitude} {Units}";

    public static bool operator <(DvQuantity left, DvQuantity right) => left.CompareTo(right) < 0;
    public static bool operator >(DvQuantity left, DvQuantity right) => left.CompareTo(right) > 0;
    public static bool operator <=(DvQuantity left, DvQuantity right) => left.CompareTo(right) <= 0;
    public static bool operator >=(DvQuantity left, DvQuantity right) => left.CompareTo(right) >= 0;
}
