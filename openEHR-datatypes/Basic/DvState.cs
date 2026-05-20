using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Basic;

/// <summary>
/// For representing state values which obey a defined state machine, such that instances
/// represent individual state machine states.
/// openEHR RM 1.1.0 data_types.basic.DV_STATE
/// </summary>
public sealed class DvState : DataValue, IEquatable<DvState>
{
    public DvCodedText Value { get; }
    public bool IsTerminal { get; }

    public DvState(DvCodedText value, bool isTerminal = false)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        IsTerminal = isTerminal;
    }

    public bool Equals(DvState? other) =>
        other is not null &&
        Value.Equals(other.Value) &&
        IsTerminal == other.IsTerminal;

    public override bool Equals(object? obj) => obj is DvState s && Equals(s);
    public override int GetHashCode() => HashCode.Combine(Value, IsTerminal);
    public override string ToString() => Value.ToString();
}
