using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Encapsulated;

/// <summary>
/// Encapsulated data expressed as a parsable string, with information about its syntax.
/// openEHR RM 1.1.0 data_types.encapsulated.DV_PARSABLE
/// </summary>
public sealed class DvParsable : DvEncapsulated
{
    public string Value { get; }
    public string Formalism { get; }

    public override int Size => System.Text.Encoding.UTF8.GetByteCount(Value);

    public DvParsable(
        string value,
        string formalism,
        CodePhrase? language = null,
        CodePhrase? charset = null)
        : base(language, charset)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value must not be null or empty.", nameof(value));
        if (string.IsNullOrEmpty(formalism))
            throw new ArgumentException("Formalism must not be null or empty.", nameof(formalism));

        Value = value;
        Formalism = formalism;
    }

    public override string ToString() => Value;
}
