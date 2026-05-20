namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// A fully coordinated term from a terminology service.
/// openEHR RM 1.1.0 data_types.text.CODE_PHRASE
/// </summary>
public sealed class CodePhrase : IEquatable<CodePhrase>
{
    public TerminologyId TerminologyId { get; }
    public string CodeString { get; }
    public string? PreferredTerm { get; }

    public CodePhrase(TerminologyId terminologyId, string codeString, string? preferredTerm = null)
    {
        TerminologyId = terminologyId ?? throw new ArgumentNullException(nameof(terminologyId));
        if (string.IsNullOrEmpty(codeString))
            throw new ArgumentException("Code string must not be null or empty.", nameof(codeString));
        CodeString = codeString;
        PreferredTerm = preferredTerm;
    }

    public bool Equals(CodePhrase? other) =>
        other is not null &&
        TerminologyId == other.TerminologyId &&
        CodeString == other.CodeString;

    public override bool Equals(object? obj) => obj is CodePhrase c && Equals(c);
    public override int GetHashCode() => HashCode.Combine(TerminologyId, CodeString);
    public override string ToString() => $"{TerminologyId}::{CodeString}";

    public static bool operator ==(CodePhrase? left, CodePhrase? right) =>
        left is null ? right is null : left.Equals(right);
    public static bool operator !=(CodePhrase? left, CodePhrase? right) => !(left == right);
}
