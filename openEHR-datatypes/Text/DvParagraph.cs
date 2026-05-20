using OpenEHR.RM.DataTypes.Basic;

namespace OpenEHR.RM.DataTypes.Text;

/// <summary>
/// A logical paragraph containing one or more DV_TEXT items.
/// Deprecated in openEHR RM 1.1.0.
/// openEHR RM 1.1.0 data_types.text.DV_PARAGRAPH
/// </summary>
[Obsolete("DV_PARAGRAPH is deprecated in openEHR RM 1.1.0. Use DV_TEXT with appropriate formatting instead.")]
public sealed class DvParagraph : DataValue
{
    public IReadOnlyList<DvText> Items { get; }

    public DvParagraph(IReadOnlyList<DvText> items)
    {
        if (items is null || items.Count == 0)
            throw new ArgumentException("Items must contain at least one DV_TEXT item.", nameof(items));
        Items = items;
    }
}
