namespace Abstractions;

public class PlayerBase
{
	public string Name { get; init; } = default!;
	public bool IsHuman { get; set; }
	public bool IsActive { get; set; }

	public override bool Equals(object? obj) => obj is PlayerBase other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
