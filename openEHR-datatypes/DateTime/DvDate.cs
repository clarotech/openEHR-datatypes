using System.Text.RegularExpressions;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.DateTime;

/// <summary>
/// Represents an ISO 8601 date. Supports partial dates (YYYY, YYYY-MM, YYYY-MM-DD).
/// openEHR RM 1.1.0 data_types.date_time.DV_DATE
/// </summary>
public sealed class DvDate : DvTemporal, IEquatable<DvDate>, IComparable<DvDate>
{
    // Supports YYYY, YYYY-MM, YYYY-MM-DD
    private static readonly Regex DateRegex = new(
        @"^(?<year>\d{4})(?:-(?<month>0[1-9]|1[0-2])(?:-(?<day>0[1-9]|[12]\d|3[01]))?)?$",
        RegexOptions.Compiled);

    public int Year { get; }
    public int? Month { get; }
    public int? Day { get; }

    public override double Magnitude =>
        Year * 365.25 + (Month ?? 1) * 30.4375 + (Day ?? 1);

    public DvDate(
        string value,
        DvDuration? accuracy = null,
        string? magnitudeStatus = null,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(value, accuracy, magnitudeStatus, normalRange, normalStatus, otherReferenceRanges)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value must not be null or empty.", nameof(value));
        var match = DateRegex.Match(value);
        if (!match.Success)
            throw new ArgumentException($"'{value}' is not a valid ISO 8601 date.", nameof(value));

        Year = int.Parse(match.Groups["year"].Value);
        Month = match.Groups["month"].Success ? int.Parse(match.Groups["month"].Value) : null;
        Day = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value) : null;
    }

    public bool IsPartial => Month is null || Day is null;

    /// <summary>True if <paramref name="other"/> is a DvDate (same comparable type).</summary>
    public override bool IsStrictlyComparableTo(DvOrdered other) => other is DvDate;

    /// <summary>Converts to System.DateOnly if the date is complete.</summary>
    public DateOnly ToDateOnly()
    {
        if (IsPartial)
            throw new InvalidOperationException("Cannot convert a partial date to DateOnly.");
        return new DateOnly(Year, Month!.Value, Day!.Value);
    }

    public override DvAbsoluteQuantity<DvDuration> AddDelta(DvDuration delta)
    {
        if (IsPartial)
            throw new InvalidOperationException("Cannot add duration to a partial date.");
        var d = ToDateOnly().AddDays(delta.Days + delta.Weeks * 7)
                             .AddMonths(delta.Months)
                             .AddYears(delta.Years);
        return new DvDate(d.ToString("yyyy-MM-dd"));
    }

    public override DvDuration Diff(DvAbsoluteQuantity<DvDuration> other)
    {
        if (other is not DvDate o)
            throw new ArgumentException("Can only diff DvDate with DvDate.", nameof(other));
        double diffDays = Magnitude - o.Magnitude;
        bool neg = diffDays < 0;
        int days = (int)Math.Abs(diffDays);
        return DvDuration.FromComponents(days: days, negative: neg);
    }

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvDate d) return Magnitude.CompareTo(d.Magnitude);
        throw new ArgumentException($"Cannot compare DvDate with {other.GetType().Name}.");
    }

    public int CompareTo(DvDate? other) => other is null ? 1 : Magnitude.CompareTo(other.Magnitude);

    public bool Equals(DvDate? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvDate d && Equals(d);
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator <(DvDate left, DvDate right) => left.CompareTo(right) < 0;
    public static bool operator >(DvDate left, DvDate right) => left.CompareTo(right) > 0;
    public static bool operator <=(DvDate left, DvDate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(DvDate left, DvDate right) => left.CompareTo(right) >= 0;
}
