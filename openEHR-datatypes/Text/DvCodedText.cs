using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// A text item whose value must be the rubric from a controlled terminology.
/// openEHR RM 1.1.0 data_types.text.DV_CODED_TEXT
/// </summary>
public sealed class DvCodedText : DvText, IEquatable<DvCodedText>
{
    public CodePhrase DefiningCode { get; }

    public DvCodedText(
        string value,
        CodePhrase definingCode,
        DvUri? hyperlink = null,
        string? formatting = null,
        IReadOnlyList<TermMapping>? mappings = null,
        CodePhrase? language = null,
        CodePhrase? encoding = null)
        : base(value, hyperlink, formatting, mappings, language, encoding)
    {
        DefiningCode = definingCode ?? throw new ArgumentNullException(nameof(definingCode));
    }

    public bool Equals(DvCodedText? other) =>
        other is not null &&
        Value == other.Value &&
        DefiningCode == other.DefiningCode;

    public override bool Equals(DvText? other) => other is DvCodedText c && Equals(c);
    public override bool Equals(object? obj) => obj is DvCodedText c && Equals(c);
    public override int GetHashCode() => HashCode.Combine(Value, DefiningCode);
}
