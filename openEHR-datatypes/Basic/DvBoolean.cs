namespace OpenEHR.RM.DataTypes.Basic;

/// <summary>
/// Items which are true or false.
/// openEHR RM 1.1.0 data_types.basic.DV_BOOLEAN
/// </summary>
public sealed class DvBoolean : DataValue, IEquatable<DvBoolean>
{
    public bool Value { get; }

    public DvBoolean(bool value)
    {
        Value = value;
    }

    public bool Equals(DvBoolean? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DvBoolean b && Equals(b);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
