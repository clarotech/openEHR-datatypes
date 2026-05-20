using System.Text.RegularExpressions;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.DateTime;

/// <summary>
/// Represents an ISO 8601 time. Supports partial times and timezone offset.
/// openEHR RM 1.1.0 data_types.date_time.DV_TIME
/// </summary>
public sealed class DvTime : DvTemporal, IEquatable<DvTime>, IComparable<DvTime>
{
    // HH, HH:MM, HH:MM:SS[.fff][Z|+HH:MM|-HH:MM]
    private static readonly Regex TimeRegex = new(
        @"^(?<hour>[01]\d|2[0-3])(?::(?<minute>[0-5]\d)(?::(?<second>[0-5]\d)(?<frac>\.\d+)?)?" +
        @"(?<tz>Z|[+-](?:[01]\d|2[0-3]):[0-5]\d)?)?$",
        RegexOptions.Compiled);

    public int Hour { get; }
    public int? Minute { get; }
    public int? Second { get; }
    public double? FractionalSecond { get; }
    public string? Timezone { get; }

    public bool HasTimezone => Timezone is not null;
    public bool IsPartial => Minute is null || Second is null;

    /// <summary>True if <paramref name="other"/> is a DvTime (same comparable type).</summary>
    public override bool IsStrictlyComparableTo(DvOrdered other) => other is DvTime;

    public override double Magnitude =>
        Hour * 3600 + (Minute ?? 0) * 60 + (Second ?? 0) + (FractionalSecond ?? 0);

    public DvTime(
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
        var match = TimeRegex.Match(value);
        if (!match.Success)
            throw new ArgumentException($"'{value}' is not a valid ISO 8601 time.", nameof(value));

        Hour = int.Parse(match.Groups["hour"].Value);
        Minute = match.Groups["minute"].Success ? int.Parse(match.Groups["minute"].Value) : null;
        Second = match.Groups["second"].Success ? int.Parse(match.Groups["second"].Value) : null;
        FractionalSecond = match.Groups["frac"].Success
            ? double.Parse("0" + match.Groups["frac"].Value)
            : null;
        Timezone = match.Groups["tz"].Success ? match.Groups["tz"].Value : null;
    }

    public override DvAbsoluteQuantity<DvDuration> AddDelta(DvDuration delta)
    {
        double totalSecs = Magnitude + delta.Hours * 3600 + delta.Minutes * 60 + delta.Seconds;
        totalSecs = ((totalSecs % 86400) + 86400) % 86400;
        int h = (int)(totalSecs / 3600); totalSecs -= h * 3600;
        int m = (int)(totalSecs / 60); double s = totalSecs - m * 60;
        string tz = Timezone ?? "";
        return new DvTime($"{h:D2}:{m:D2}:{s:00.###}{tz}");
    }

    public override DvDuration Diff(DvAbsoluteQuantity<DvDuration> other)
    {
        if (other is not DvTime o)
            throw new ArgumentException("Can only diff DvTime with DvTime.", nameof(other));
        double diff = Magnitude - o.Magnitude;
        bool neg = diff < 0;
        double absDiff = Math.Abs(diff);
        int h = (int)(absDiff / 3600); absDiff -= h * 3600;
        int m = (int)(absDiff / 60); double s = absDiff - m * 60;
        return DvDuration.FromComponents(hours: h, minutes: m, seconds: s, negative: neg);
    }

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvTime t) return Magnitude.CompareTo(t.Magnitude);
        throw new ArgumentException($"Cannot compare DvTime with {other.GetType().Name}.");
    }

    public int CompareTo(DvTime? other) => other is null ? 1 : Magnitude.CompareTo(other.Magnitude);

    public bool Equals(DvTime? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvTime t && Equals(t);
    public override int GetHashCode() => Value.GetHashCode();
}
