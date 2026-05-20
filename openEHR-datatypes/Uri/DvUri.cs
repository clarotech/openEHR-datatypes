using OpenEHR.RM.DataTypes.Basic;

namespace OpenEHR.RM.DataTypes.Uri;

/// <summary>
/// A reference to an object which structurally conforms to the URI RFC.
/// openEHR RM 1.1.0 data_types.uri.DV_URI
/// </summary>
public class DvUri : DataValue, IEquatable<DvUri>
{
    private readonly System.Uri _uri;

    public string Value { get; }

    public DvUri(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value must not be null or empty.", nameof(value));
        if (!System.Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
            throw new ArgumentException($"'{value}' is not a valid URI.", nameof(value));
        Value = value;
        _uri = uri;
    }

    public string? Scheme => _uri.IsAbsoluteUri ? _uri.Scheme : null;
    public string? Path => _uri.IsAbsoluteUri ? _uri.AbsolutePath : _uri.OriginalString;
    public string? Query => _uri.IsAbsoluteUri && _uri.Query.Length > 0 ? _uri.Query.TrimStart('?') : null;
    public string? Fragment => _uri.IsAbsoluteUri && _uri.Fragment.Length > 0 ? _uri.Fragment.TrimStart('#') : null;

    public bool Equals(DvUri? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvUri u && Equals(u);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
