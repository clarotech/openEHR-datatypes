namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// Represents a mapping of one term to another, in the same or different terminology.
/// openEHR RM 1.1.0 data_types.text.TERM_MAPPING
/// </summary>
public sealed class TermMapping
{
    private static readonly HashSet<char> ValidMatchChars = new() { '>', '=', '<', '?' };

    /// <summary>
    /// The match operator: '>' (broader), '=' (equivalent), '&lt;' (narrower), '?' (unknown).
    /// </summary>
    public char Match { get; }

    /// <summary>
    /// Optional purpose of this mapping.
    /// </summary>
    public DvCodedText? Purpose { get; }

    /// <summary>
    /// The target term of this mapping.
    /// </summary>
    public CodePhrase Target { get; }

    public TermMapping(char match, CodePhrase target, DvCodedText? purpose = null)
    {
        if (!ValidMatchChars.Contains(match))
            throw new ArgumentException(
                $"'{match}' is not a valid match character. Must be one of: >, =, <, ?",
                nameof(match));
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Match = match;
        Purpose = purpose;
    }

    /// <summary>True if this mapping is broader ('>') than the mapped-to term.</summary>
    public bool Broader() => Match == '>';

    /// <summary>True if this mapping is equivalent ('=') to the mapped-to term.</summary>
    public bool Equivalent() => Match == '=';

    /// <summary>True if this mapping is narrower ('&lt;') than the mapped-to term.</summary>
    public bool Narrower() => Match == '<';

    /// <summary>True if the mapping relationship is unknown ('?').</summary>
    public bool Unknown() => Match == '?';
}
