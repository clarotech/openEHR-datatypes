using System.Text.RegularExpressions;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;

namespace OpenEHR.RM.DataTypes.DateTime;

/// <summary>
/// Represents a duration of time in ISO 8601 format. Supports negative durations.
/// openEHR RM 1.1.0 data_types.date_time.DV_DURATION
/// </summary>
public sealed class DvDuration : DvAmount, IEquatable<DvDuration>, IComparable<DvDuration>
{
    // Matches: [-]P[nY][nM][nW][nD][T[nH][nM][n[.n]S]]
    private static readonly Regex DurationRegex = new(
        @"^(?<neg>-)?P(?:(?<years>\d+)Y)?(?:(?<months>\d+)M)?(?:(?<weeks>\d+)W)?(?:(?<days>\d+)D)?" +
        @"(?:T(?:(?<hours>\d+)H)?(?:(?<minutes>\d+)M)?(?:(?<seconds>\d+(?:\.\d+)?)S)?)?$",
        RegexOptions.Compiled);

    public string Value { get; }

    public bool IsNegative { get; }
    public int Years { get; }
    public int Months { get; }
    public int Weeks { get; }
    public int Days { get; }
    public int Hours { get; }
    public int Minutes { get; }
    public double Seconds { get; }

    public override double Magnitude =>
        (IsNegative ? -1 : 1) *
        (Years * 365.25 * 86400 +
         Months * 30.4375 * 86400 +
         Weeks * 7 * 86400 +
         Days * 86400 +
         Hours * 3600 +
         Minutes * 60 +
         Seconds);

    public DvDuration(
        string value,
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(magnitudeStatus, accuracy, accuracyIsPercent, normalRange, normalStatus, otherReferenceRanges)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value must not be null or empty.", nameof(value));
        var match = DurationRegex.Match(value);
        if (!match.Success)
            throw new ArgumentException($"'{value}' is not a valid ISO 8601 duration.", nameof(value));
        // ISO 8601 requires at least one designator (bare "P" or "-P" is invalid)
        var componentGroups = new[] { "years", "months", "weeks", "days", "hours", "minutes", "seconds" };
        if (!componentGroups.Any(g => match.Groups[g].Success))
            throw new ArgumentException($"'{value}' is not a valid ISO 8601 duration: at least one component is required.", nameof(value));

        Value = value;
        IsNegative = match.Groups["neg"].Success;

        static int ParseGroup(Group g) => g.Success ? int.Parse(g.Value) : 0;
        static double ParseSeconds(Group g) => g.Success ? double.Parse(g.Value) : 0.0;

        Years = ParseGroup(match.Groups["years"]);
        Months = ParseGroup(match.Groups["months"]);
        Weeks = ParseGroup(match.Groups["weeks"]);
        Days = ParseGroup(match.Groups["days"]);
        Hours = ParseGroup(match.Groups["hours"]);
        Minutes = ParseGroup(match.Groups["minutes"]);
        Seconds = ParseSeconds(match.Groups["seconds"]);
    }

    /// <summary>Creates a DvDuration from component values.</summary>
    public static DvDuration FromComponents(int years = 0, int months = 0, int weeks = 0, int days = 0,
        int hours = 0, int minutes = 0, double seconds = 0, bool negative = false)
    {
        var sb = new System.Text.StringBuilder();
        if (negative) sb.Append('-');
        sb.Append('P');
        if (years != 0) sb.Append($"{years}Y");
        if (months != 0) sb.Append($"{months}M");
        if (weeks != 0) sb.Append($"{weeks}W");
        if (days != 0) sb.Append($"{days}D");
        bool hasTime = hours != 0 || minutes != 0 || seconds != 0;
        if (hasTime)
        {
            sb.Append('T');
            if (hours != 0) sb.Append($"{hours}H");
            if (minutes != 0) sb.Append($"{minutes}M");
            if (seconds != 0) sb.Append($"{seconds}S");
        }
        if (sb.ToString() == "P" || sb.ToString() == "-P") sb.Append("0D");
        return new DvDuration(sb.ToString());
    }

    /// <summary>True if <paramref name="other"/> is a DvDuration (same comparable type).</summary>
    public override bool IsStrictlyComparableTo(DvOrdered other) => other is DvDuration;

    public override DvAmount ArithmeticAdd(DvAmount other)
    {
        if (other is not DvDuration d)
            throw new ArgumentException("Can only add DvDuration to DvDuration.", nameof(other));
        double totalSeconds = Magnitude + d.Magnitude;
        bool neg = totalSeconds < 0;
        totalSeconds = Math.Abs(totalSeconds);
        int w = (int)(totalSeconds / 604800); totalSeconds -= w * 604800;
        int dy = (int)(totalSeconds / 86400); totalSeconds -= dy * 86400;
        int h = (int)(totalSeconds / 3600); totalSeconds -= h * 3600;
        int m = (int)(totalSeconds / 60); double s = totalSeconds - m * 60;
        return FromComponents(weeks: w, days: dy, hours: h, minutes: m, seconds: s, negative: neg);
    }

    public override DvAmount Negate()
    {
        string negated = Value.StartsWith('-') ? Value[1..] : "-" + Value;
        return new DvDuration(negated, MagnitudeStatus, Accuracy, AccuracyIsPercent);
    }

    public override int CompareTo(DvOrdered? other)
    {
        if (other is null) return 1;
        if (other is DvDuration d) return Magnitude.CompareTo(d.Magnitude);
        throw new ArgumentException($"Cannot compare DvDuration with {other.GetType().Name}.");
    }

    public int CompareTo(DvDuration? other) => other is null ? 1 : Magnitude.CompareTo(other.Magnitude);

    public bool Equals(DvDuration? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvDuration d && Equals(d);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
