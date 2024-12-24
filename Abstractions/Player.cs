using System.Diagnostics;

namespace Abstractions;

[DebuggerDisplay("{Name}{HumanStatus}")]
public class Player
{
	public string Name { get; init; } = default!;
	public bool IsHuman { get; init; }
	public bool IsActive { get; internal set; }

	private string HumanStatus => !IsHuman ? ", bot" : string.Empty;

	public override bool Equals(object? obj) => obj is Player other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
