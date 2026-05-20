namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Generic interval of an ordered type. Lower and/or upper may be open (unbounded).
/// openEHR RM 1.1.0 data_types.quantity.DV_INTERVAL
/// </summary>
public sealed class DvInterval<T> where T : class, IComparable<T>
{
    public T? Lower { get; }
    public T? Upper { get; }
    public bool LowerUnbounded { get; }
    public bool UpperUnbounded { get; }
    public bool LowerIncluded { get; }
    public bool UpperIncluded { get; }

    public DvInterval(
        T? lower,
        T? upper,
        bool lowerUnbounded = false,
        bool upperUnbounded = false,
        bool lowerIncluded = true,
        bool upperIncluded = true)
    {
        if (lowerUnbounded && lower is not null)
            throw new ArgumentException("Lower must be null when LowerUnbounded is true.", nameof(lower));
        if (upperUnbounded && upper is not null)
            throw new ArgumentException("Upper must be null when UpperUnbounded is true.", nameof(upper));
        if (!lowerUnbounded && lower is not null && !upperUnbounded && upper is not null)
        {
            if (lower.CompareTo(upper) > 0)
                throw new ArgumentException("Lower must be less than or equal to Upper.");
        }

        Lower = lower;
        Upper = upper;
        LowerUnbounded = lowerUnbounded;
        UpperUnbounded = upperUnbounded;
        LowerIncluded = lowerIncluded;
        UpperIncluded = upperIncluded;
    }

    /// <summary>Unbounded interval containing all values.</summary>
    public static DvInterval<T> Unbounded() =>
        new(default, default, lowerUnbounded: true, upperUnbounded: true);

    /// <summary>Returns true if value falls within this interval.</summary>
    public bool Has(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        bool lowerOk = LowerUnbounded || Lower is null ||
            (LowerIncluded ? value.CompareTo(Lower) >= 0 : value.CompareTo(Lower) > 0);

        bool upperOk = UpperUnbounded || Upper is null ||
            (UpperIncluded ? value.CompareTo(Upper) <= 0 : value.CompareTo(Upper) < 0);

        return lowerOk && upperOk;
    }

    /// <summary>Returns true if this interval overlaps with other.</summary>
    public bool Intersects(DvInterval<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (LowerUnbounded || other.UpperUnbounded) { /* check other side */ }
        if (UpperUnbounded || other.LowerUnbounded) { /* check other side */ }

        bool thisStartsBeforeOtherEnds =
            UpperUnbounded || other.LowerUnbounded ||
            (Upper is not null && other.Lower is not null &&
             (UpperIncluded && other.LowerIncluded
                 ? Upper.CompareTo(other.Lower) >= 0
                 : Upper.CompareTo(other.Lower) > 0));

        bool otherStartsBeforeThisEnds =
            other.UpperUnbounded || LowerUnbounded ||
            (other.Upper is not null && Lower is not null &&
             (other.UpperIncluded && LowerIncluded
                 ? other.Upper.CompareTo(Lower) >= 0
                 : other.Upper.CompareTo(Lower) > 0));

        return thisStartsBeforeOtherEnds && otherStartsBeforeThisEnds;
    }

    /// <summary>Returns true if this interval completely contains other.</summary>
    public bool Contains(DvInterval<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return (other.Lower is null || other.LowerUnbounded || Has(other.Lower)) &&
               (other.Upper is null || other.UpperUnbounded || Has(other.Upper));
    }
}
