namespace BlazorApp.Components.Games.Mini2PGame;

public class Piece : Abstractions.Piece
{
	public required string Name { get; init; }

	public override string ToString() => $"{PlayerName}.{Name}";

	public override int GetHashCode() => ToString().GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => 
		obj is Piece other &&
		PlayerName.Equals(other.PlayerName, StringComparison.OrdinalIgnoreCase) &&
		Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
}
