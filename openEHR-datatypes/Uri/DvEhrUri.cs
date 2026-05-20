namespace OpenEHR.RM.DataTypes.Uri;

/// <summary>
/// A DV_URI type specifically for EHR URIs, using the ehr:// scheme.
/// openEHR RM 1.1.0 data_types.uri.DV_EHR_URI
/// </summary>
public sealed class DvEhrUri : DvUri
{
    private const string EhrScheme = "ehr";

    public DvEhrUri(string value) : base(value)
    {
        if (Scheme != EhrScheme)
            throw new ArgumentException(
                $"EHR URI must use the 'ehr' scheme, got '{Scheme}'.", nameof(value));
    }

    /// <summary>
    /// The EHR id segment of the URI path (first path component).
    /// e.g. ehr://system.example.com/{ehr_id}/...
    /// </summary>
    public string? EhrId
    {
        get
        {
            var path = Path?.TrimStart('/');
            if (string.IsNullOrEmpty(path)) return null;
            var segments = path.Split('/');
            return segments.Length > 0 ? segments[0] : null;
        }
    }
}
