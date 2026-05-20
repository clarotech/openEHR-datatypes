using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Encapsulated;

/// <summary>
/// Abstract class defining the common meta-data of all types of encapsulated data.
/// openEHR RM 1.1.0 data_types.encapsulated.DV_ENCAPSULATED
/// </summary>
public abstract class DvEncapsulated : DataValue
{
    public CodePhrase? Language { get; }
    public CodePhrase? Charset { get; }

    protected DvEncapsulated(CodePhrase? language = null, CodePhrase? charset = null)
    {
        Language = language;
        Charset = charset;
    }

    /// <summary>Size in bytes of the encapsulated data.</summary>
    public abstract int Size { get; }
}
