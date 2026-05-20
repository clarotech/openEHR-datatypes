using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// A text item, which may contain any amount of legal characters arranged as e.g. words, sentences etc.
/// openEHR RM 1.1.0 data_types.text.DV_TEXT
/// </summary>
public class DvText : DataValue, IEquatable<DvText>
{
    private static readonly HashSet<string> ValidFormattingValues =
        new() { "plain", "plain_no_newlines", "markdown" };

    public string Value { get; }

    /// <summary>Optional link to an external resource (deprecated in RM 1.1.0).</summary>
    public DvUri? Hyperlink { get; }

    /// <summary>
    /// Optional formatting hint. Valid values: "plain", "plain_no_newlines", "markdown".
    /// </summary>
    public string? Formatting { get; }

    public IReadOnlyList<TermMapping>? Mappings { get; }
    public CodePhrase? Language { get; }
    public CodePhrase? Encoding { get; }

    public DvText(
        string value,
        DvUri? hyperlink = null,
        string? formatting = null,
        IReadOnlyList<TermMapping>? mappings = null,
        CodePhrase? language = null,
        CodePhrase? encoding = null)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value must not be null or empty.", nameof(value));
        if (formatting is not null && !ValidFormattingValues.Contains(formatting))
            throw new ArgumentException(
                $"'{formatting}' is not a valid formatting value. Valid values: {string.Join(", ", ValidFormattingValues)}",
                nameof(formatting));

        Value = value;
        Hyperlink = hyperlink;
        Formatting = formatting;
        Mappings = mappings;
        Language = language;
        Encoding = encoding;
    }

    public virtual bool Equals(DvText? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is DvText t && Equals(t);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
