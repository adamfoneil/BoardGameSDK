using System.Diagnostics;

namespace Abstractions;

[DebuggerDisplay("{Name}")]
public class PlayerBase
{
	public string Name { get; init; } = default!;
	public bool IsHuman { get; init; }
	public bool IsActive { get; internal set; }

	public override bool Equals(object? obj) => obj is PlayerBase other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
