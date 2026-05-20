namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// Identifier of a terminology.
/// openEHR RM 1.1.0 support.terminology.TERMINOLOGY_ID
/// </summary>
public sealed class TerminologyId : IEquatable<TerminologyId>
{
    public string Value { get; }

    public TerminologyId(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Terminology ID value must not be null or empty.", nameof(value));
        Value = value;
    }

    public bool Equals(TerminologyId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is TerminologyId t && Equals(t);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(TerminologyId? left, TerminologyId? right) =>
        left is null ? right is null : left.Equals(right);
    public static bool operator !=(TerminologyId? left, TerminologyId? right) => !(left == right);
}
