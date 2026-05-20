namespace OpenEHR.RM.DataTypes.Basic;

/// <summary>
/// Type for representing identifiers of real-world entities.
/// openEHR RM 1.1.0 data_types.basic.DV_IDENTIFIER
/// </summary>
public sealed class DvIdentifier : DataValue, IEquatable<DvIdentifier>
{
    public string Id { get; }
    public string? Issuer { get; }
    public string? Assigner { get; }
    public string? Type { get; }

    public DvIdentifier(string id, string? issuer = null, string? assigner = null, string? type = null)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("Id must not be null or empty.", nameof(id));
        Id = id;
        Issuer = issuer;
        Assigner = assigner;
        Type = type;
    }

    public bool Equals(DvIdentifier? other) =>
        other is not null &&
        Id == other.Id &&
        Issuer == other.Issuer &&
        Assigner == other.Assigner &&
        Type == other.Type;

    public override bool Equals(object? obj) => obj is DvIdentifier d && Equals(d);
    public override int GetHashCode() => HashCode.Combine(Id, Issuer, Assigner, Type);
    public override string ToString() => Id;
}
