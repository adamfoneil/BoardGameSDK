using Abstractions;
using System.Diagnostics;

namespace BlazorApp.Components.Games.Mini1PGame;

[DebuggerDisplay("{Name}")]
public class MiniGamePlayer : Player
{
	public override string ToString() => Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => obj is MiniGamePlayer player && Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase);	
}

[DebuggerDisplay("{Name}")]
public class MiniGamePiece : Piece
{
	public required string Name { get; init; }

	public override string ToString() => Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => obj is MiniGamePiece other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

	#region maybe-later
	public static Dictionary<char, string[]> DefaultNames => new()
	{
		['a'] = ["Ajax", "Acrid", "Avon", "Anvil"],
		['b'] = ["Brunswick", "Banjo", "Boom", "Bronze"],
		['c'] = ["Comet", "Cathode", "Carbon", "Cathode"],
		['d'] = ["Doodle", "Damsel", "Darius", "Doofus"],
		['e'] = ["Ember", "Engine", "Euclid", "Enzo"],
		['f'] = ["Fancy", "Fidelio", "Fulsome", "Fenway"],
		['g'] = ["Garlic", "Gallium", "Galax", "Galway"]
	};
	#endregion
}

/// <summary>
/// you have 3 pieces, and you can move up to 5 spaces on each turn across your pieces, but not overlapping.
/// single player with no objective besides taking 7 turns
/// </summary>
public class MiniGameState : GameState<MiniGamePlayer, MiniGamePiece>
{
	public const uint DefinedWidth = 20;
	public const uint DefinedHeight = 20;

	public const int SpacesPerTurn = 5;
	public const int TurnsPerGame = 7;

	public override uint Width { get; } = DefinedWidth;
	public override uint Height { get; } = DefinedHeight;

	private int _spacesMoved = 0;

	protected override Location[] GetValidMovesInner(MiniGamePlayer player, MiniGamePiece piece) =>
		piece.Location.GetAdjacentLocations(Directions.All, SpacesPerTurn - _spacesMoved).ToArray();
	
	protected override (string? logTemplate, object?[] logParams) PlayInner(MiniGamePlayer player, MiniGamePiece piece, Location location)
	{
		int distance = piece.Location.Distance(location);
		_spacesMoved += distance;

		piece.X = location.X;
		piece.Y = location.Y;
				
		return ("{player} moved {piece} {spaces} to {location}", [ player, piece, distance, location ]);
	}

	protected override (bool result, string? reason) ValidateInner(MiniGamePlayer player, MiniGamePiece piece, Location location)
	{
		// no capturing in this game, just movement
		if (PiecesByLocation.ContainsKey(location)) return (false, "Piece already there");
		return (true, default);
	}
}
