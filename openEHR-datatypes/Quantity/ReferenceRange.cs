using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Defines a named range to be associated with any DV_ORDERED datum.
/// openEHR RM 1.1.0 data_types.quantity.REFERENCE_RANGE
/// </summary>
public sealed class ReferenceRange<T> where T : DvOrdered
{
    public DvInterval<T> Range { get; }
    public DvText? Meaning { get; }
    public DvText? Instruction { get; }

    public ReferenceRange(DvInterval<T> range, DvText? meaning = null, DvText? instruction = null)
    {
        Range = range ?? throw new ArgumentNullException(nameof(range));
        Meaning = meaning;
        Instruction = instruction;
    }

    /// <summary>True if value falls within this reference range.</summary>
    public bool IsInRange(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Range.Has(value);
    }
}
