using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Abstract class representing values with a computable magnitude.
/// openEHR RM 1.1.0 data_types.quantity.DV_QUANTIFIED
/// </summary>
public abstract class DvQuantified : DvOrdered
{
    /// <summary>
    /// Optional status of the magnitude, e.g. "=" (exact), "&lt;" (less than), "~" (approximate).
    /// </summary>
    public string? MagnitudeStatus { get; }

    /// <summary>Optional accuracy of the magnitude as a value.</summary>
    public double? Accuracy { get; }

    /// <summary>True if accuracy is expressed as a percent of the magnitude.</summary>
    public bool AccuracyIsPercent { get; }

    protected DvQuantified(
        string? magnitudeStatus = null,
        double? accuracy = null,
        bool accuracyIsPercent = false,
        ReferenceRange<DvOrdered>? normalRange = null,
        CodePhrase? normalStatus = null,
        IReadOnlyList<ReferenceRange<DvOrdered>>? otherReferenceRanges = null)
        : base(normalRange, normalStatus, otherReferenceRanges)
    {
        Quantity.MagnitudeStatus.Validate(magnitudeStatus);
        if (accuracy is < 0)
            throw new ArgumentOutOfRangeException(nameof(accuracy), "Accuracy must not be negative.");
        if (accuracyIsPercent && accuracy is > 100)
            throw new ArgumentOutOfRangeException(nameof(accuracy), "Percent accuracy must be between 0 and 100.");

        MagnitudeStatus = magnitudeStatus;
        Accuracy = accuracy;
        AccuracyIsPercent = accuracyIsPercent;
    }

    /// <summary>The numeric magnitude of this quantity.</summary>
    public abstract double Magnitude { get; }

    /// <summary>True if magnitude is exactly known (status is null or "=").</summary>
    public bool MagnitudeStatusIsExact() =>
        MagnitudeStatus is null || MagnitudeStatus == Quantity.MagnitudeStatus.Equal;

    /// <summary>
    /// True if <paramref name="s"/> is a valid magnitude status string (null is also valid,
    /// meaning the status is unset).
    /// openEHR RM 1.1.0: DV_QUANTIFIED.valid_magnitude_status
    /// </summary>
    public static bool ValidMagnitudeStatus(string? s) => Quantity.MagnitudeStatus.IsValid(s);
}
