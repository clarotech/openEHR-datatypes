using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Models a ratio of values, i.e. where the numerator and denominator are both pure numbers.
/// openEHR RM 1.1.0 data_types.quantity.DV_PROPORTION
/// </summary>
public sealed class DvProportion : DvAmount, IEquatable<DvProportion>, IComparable<DvProportion>
{
    public double Numerator { get; }
    public double Denominator { get; }
    public ProportionKind Type { get; }
    public int? Precision { get; }

    public override double Magnitude => Denominator == 0 ? 0 : Numerator / Denominator;

    public DvProportion(
        double numerator,
        double denominator,
        ProportionKind type,
        int? precision = null,
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, accuracy, accuracyIsPercent, normalRange, normalStatus, otherReferenceRanges)
    {
        ValidateForType(numerator, denominator, type);
        Numerator = numerator;
        Denominator = denominator;
        Type = type;
        Precision = precision;
    }

    private static void ValidateForType(double numerator, double denominator, ProportionKind type)
    {
        switch (type)
        {
            case ProportionKind.Unitary:
                if (denominator != 1.0)
                    throw new ArgumentException("Unitary proportion must have denominator = 1.", nameof(denominator));
                break;
            case ProportionKind.Percent:
                if (denominator != 100.0)
                    throw new ArgumentException("Percent proportion must have denominator = 100.", nameof(denominator));
                break;
            case ProportionKind.UnitaryRange:
                if (denominator != 1.0)
                    throw new ArgumentException("UnitaryRange proportion must have denominator = 1.", nameof(denominator));
                if (numerator is < 0.0 or > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(numerator), "UnitaryRange numerator must be between 0.0 and 1.0.");
                break;
            case ProportionKind.Fraction:
                if (numerator != Math.Truncate(numerator))
                    throw new ArgumentException("Fraction proportion numerator must be an integer.", nameof(numerator));
                if (denominator != Math.Truncate(denominator))
                    throw new ArgumentException("Fraction proportion denominator must be an integer.", nameof(denominator));
                if (denominator == 0)
                    throw new ArgumentException("Fraction proportion denominator must not be zero.", nameof(denominator));
                break;
            case ProportionKind.Ratio:
                if (denominator == 0)
                    throw new ArgumentException("Ratio proportion denominator must not be zero.", nameof(denominator));
                break;
        }
    }

    /// <summary>True if the proportion is an integer fraction (Fraction type).</summary>
    public bool IsIntegral() => Type == ProportionKind.Fraction;

    /// <summary>
    /// True if <paramref name="other"/> is a DvProportion of the same kind, so the two
    /// values are on a common scale and can be meaningfully compared.
    /// openEHR RM 1.1.0: DV_ORDERED.is_strictly_comparable_to
    /// </summary>
    public override bool IsStrictlyComparableTo(DvOrdered other) =>
        other is DvProportion p && p.Type == Type;

    public override DvAmount ArithmeticAdd(DvAmount other)
    {
        if (other is not DvProportion p || p.Type != Type || p.Denominator != Denominator)
            throw new ArgumentException("Can only add DvProportion with the same type and denominator.", nameof(other));
        return new DvProportion(Numerator + p.Numerator, Denominator, Type, Precision);
    }

    public override DvAmount Negate() =>
        new DvProportion(-Numerator, Denominator, Type, Precision, MagnitudeStatus, Accuracy, AccuracyIsPercent);

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvProportion p) return Magnitude.CompareTo(p.Magnitude);
        throw new ArgumentException($"Cannot compare DvProportion with {other.GetType().Name}.");
    }

    public int CompareTo(DvProportion? other) => other is null ? 1 : Magnitude.CompareTo(other.Magnitude);

    public bool Equals(DvProportion? other) =>
        other is not null &&
        Numerator == other.Numerator &&
        Denominator == other.Denominator &&
        Type == other.Type;

    public override bool Equals(object? obj) => obj is DvProportion p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator, Type);

    public override string ToString() =>
        Type switch
        {
            ProportionKind.Percent => $"{Numerator}%",
            ProportionKind.Fraction => $"{(long)Numerator}/{(long)Denominator}",
            _ => $"{Numerator}/{Denominator}"
        };
}
