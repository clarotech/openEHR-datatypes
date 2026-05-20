using System.Text.RegularExpressions;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.DateTime;

/// <summary>
/// Represents an ISO 8601 date-time. Supports partial datetimes.
/// openEHR RM 1.1.0 data_types.date_time.DV_DATE_TIME
/// </summary>
public sealed class DvDateTime : DvTemporal, IEquatable<DvDateTime>, IComparable<DvDateTime>
{
    // YYYY[-MM[-DD[T...]]]
    private static readonly Regex DateTimeRegex = new(
        @"^(?<year>\d{4})(?:-(?<month>0[1-9]|1[0-2])(?:-(?<day>0[1-9]|[12]\d|3[01])" +
        @"(?:T(?<hour>[01]\d|2[0-3])(?::(?<minute>[0-5]\d)(?::(?<second>[0-5]\d)(?<frac>\.\d+)?)?" +
        @"(?<tz>Z|[+-](?:[01]\d|2[0-3]):[0-5]\d)?)?)?)?)?$",
        RegexOptions.Compiled);

    public int Year { get; }
    public int? Month { get; }
    public int? Day { get; }
    public int? Hour { get; }
    public int? Minute { get; }
    public int? Second { get; }
    public double? FractionalSecond { get; }
    public string? Timezone { get; }

    public bool IsPartial => Month is null || Day is null || Hour is null;
    public bool HasTimezone => Timezone is not null;

    public override double Magnitude =>
        Year * 365.25 * 86400 +
        (Month ?? 1) * 30.4375 * 86400 +
        (Day ?? 1) * 86400 +
        (Hour ?? 0) * 3600 +
        (Minute ?? 0) * 60 +
        (Second ?? 0) +
        (FractionalSecond ?? 0);

    public DvDateTime(
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
        var match = DateTimeRegex.Match(value);
        if (!match.Success)
            throw new ArgumentException($"'{value}' is not a valid ISO 8601 date-time.", nameof(value));

        Year = int.Parse(match.Groups["year"].Value);
        Month = match.Groups["month"].Success ? int.Parse(match.Groups["month"].Value) : null;
        Day = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value) : null;
        Hour = match.Groups["hour"].Success ? int.Parse(match.Groups["hour"].Value) : null;
        Minute = match.Groups["minute"].Success ? int.Parse(match.Groups["minute"].Value) : null;
        Second = match.Groups["second"].Success ? int.Parse(match.Groups["second"].Value) : null;
        FractionalSecond = match.Groups["frac"].Success
            ? double.Parse("0" + match.Groups["frac"].Value)
            : null;
        Timezone = match.Groups["tz"].Success ? match.Groups["tz"].Value : null;
    }

    /// <summary>Converts to System.DateTimeOffset if the datetime is complete and has a timezone.</summary>
    public DateTimeOffset ToDateTimeOffset()
    {
        if (IsPartial)
            throw new InvalidOperationException("Cannot convert a partial datetime to DateTimeOffset.");
        if (!HasTimezone)
            throw new InvalidOperationException("Cannot convert a datetime without timezone to DateTimeOffset.");
        return DateTimeOffset.Parse(Value);
    }

    public override DvAbsoluteQuantity<DvDuration> AddDelta(DvDuration delta)
    {
        if (IsPartial)
            throw new InvalidOperationException("Cannot add duration to a partial datetime.");
        var dt = new DateTimeOffset(Year, Month!.Value, Day!.Value,
            Hour!.Value, Minute ?? 0, Second ?? 0, TimeSpan.Zero);
        dt = dt.AddYears(delta.Years).AddMonths(delta.Months)
               .AddDays(delta.Days + delta.Weeks * 7)
               .AddHours(delta.Hours).AddMinutes(delta.Minutes).AddSeconds(delta.Seconds);
        return new DvDateTime(dt.ToString("yyyy-MM-ddTHH:mm:ss") + (Timezone ?? ""));
    }

    public override DvDuration Diff(DvAbsoluteQuantity<DvDuration> other)
    {
        if (other is not DvDateTime o)
            throw new ArgumentException("Can only diff DvDateTime with DvDateTime.", nameof(other));
        double diff = Magnitude - o.Magnitude;
        bool neg = diff < 0;
        double abs = Math.Abs(diff);
        int d = (int)(abs / 86400); abs -= d * 86400;
        int h = (int)(abs / 3600); abs -= h * 3600;
        int m = (int)(abs / 60); double s = abs - m * 60;
        return DvDuration.FromComponents(days: d, hours: h, minutes: m, seconds: s, negative: neg);
    }

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvDateTime dt) return Magnitude.CompareTo(dt.Magnitude);
        throw new ArgumentException($"Cannot compare DvDateTime with {other.GetType().Name}.");
    }

    public int CompareTo(DvDateTime? other) => other is null ? 1 : Magnitude.CompareTo(other.Magnitude);

    public bool Equals(DvDateTime? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvDateTime dt && Equals(dt);
    public override int GetHashCode() => Value.GetHashCode();
}
