using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Countable quantities. Used for countable types such as number of episodes.
/// openEHR RM 1.1.0 data_types.quantity.DV_COUNT
/// </summary>
public sealed class DvCount : DvAmount, IEquatable<DvCount>, IComparable<DvCount>
{
    private readonly long _magnitude;

    public override double Magnitude => _magnitude;
    public long IntegerMagnitude => _magnitude;

    public DvCount(
        long magnitude,
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, accuracy, accuracyIsPercent, normalRange, normalStatus, otherReferenceRanges)
    {
        _magnitude = magnitude;
    }

    public override DvAmount ArithmeticAdd(DvAmount other)
    {
        if (other is not DvCount c)
            throw new ArgumentException("Can only add DvCount to DvCount.", nameof(other));
        return new DvCount(_magnitude + c._magnitude);
    }

    public override DvAmount Negate() => new DvCount(-_magnitude, MagnitudeStatus, Accuracy, AccuracyIsPercent);

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvCount c) return _magnitude.CompareTo(c._magnitude);
        throw new ArgumentException($"Cannot compare DvCount with {other.GetType().Name}.");
    }

    public int CompareTo(DvCount? other) => other is null ? 1 : _magnitude.CompareTo(other._magnitude);

    public bool Equals(DvCount? other) => other is not null && _magnitude == other._magnitude;
    public override bool Equals(object? obj) => obj is DvCount c && Equals(c);
    public override int GetHashCode() => _magnitude.GetHashCode();
    public override string ToString() => _magnitude.ToString();

    public static bool operator <(DvCount left, DvCount right) => left.CompareTo(right) < 0;
    public static bool operator >(DvCount left, DvCount right) => left.CompareTo(right) > 0;
    public static bool operator <=(DvCount left, DvCount right) => left.CompareTo(right) <= 0;
    public static bool operator >=(DvCount left, DvCount right) => left.CompareTo(right) >= 0;
}
