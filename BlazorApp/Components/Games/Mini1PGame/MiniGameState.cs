using Abstractions;

namespace BlazorApp.Components.Games.Mini1PGame;

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
		piece.Location
			.GetAdjacentLocations(Directions.All, SpacesPerTurn - _spacesMoved)
			.Except(PlayerPieces[player.Name].Select(p => p.Location))
			.ToArray();
	
	protected override (string currentPlayer, string? logTemplate, object?[] logParams) PlayInner(MiniGamePlayer player, MiniGamePiece piece, Location location)
	{
		int distance = piece.Location.Distance(location);
		_spacesMoved += distance;

		piece.X = location.X;
		piece.Y = location.Y;
		
		if (_spacesMoved ==  SpacesPerTurn) _spacesMoved = 0;

		return (CurrentPlayer!, "{player} moved {piece} {spaces} spaces to {location}", [ player, piece, distance, location ]);
	}

	protected override (bool result, string? reason) ValidateInner(MiniGamePlayer player, MiniGamePiece piece, Location location)
	{
		// no capturing in this game, just movement
		if (PiecesByLocation.ContainsKey(location)) return (false, "Piece already there");
		return (true, default);
	}
}
