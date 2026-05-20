namespace OpenEHR.RM.DataTypes.Quantity;

/// <summary>
/// Enumeration of proportion types used by DvProportion.
/// openEHR RM 1.1.0 data_types.quantity.PROPORTION_KIND
/// </summary>
public enum ProportionKind
{
    Ratio = 0,
    Unitary = 1,
    Percent = 2,
    UnitaryRange = 3,
    Fraction = 4
}
