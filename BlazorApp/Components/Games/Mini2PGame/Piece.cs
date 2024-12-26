using BlazorApp.Components.Games.Mini1PGame;

namespace BlazorApp.Components.Games.Mini2PGame;

public class Piece : Abstractions.Piece
{
	public required string Name { get; init; }

	public override string ToString() => Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => obj is MiniGamePiece other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
}
