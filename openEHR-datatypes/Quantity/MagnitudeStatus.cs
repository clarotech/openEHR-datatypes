namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Valid values for DvQuantified.MagnitudeStatus.
/// </summary>
public static class MagnitudeStatus
{
    public const string Equal = "=";
    public const string LessThan = "<";
    public const string GreaterThan = ">";
    public const string LessThanOrEqual = "<=";
    public const string GreaterThanOrEqual = ">=";
    public const string Approximate = "~";

    private static readonly HashSet<string> ValidValues =
        new() { Equal, LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual, Approximate };

    public static bool IsValid(string? value) => value is null || ValidValues.Contains(value);

    public static void Validate(string? value)
    {
        if (value is not null && !ValidValues.Contains(value))
            throw new ArgumentException(
                $"'{value}' is not a valid magnitude_status. Valid values: {string.Join(", ", ValidValues)}",
                nameof(value));
    }
}
