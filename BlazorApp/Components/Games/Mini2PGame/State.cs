using Abstractions;

namespace BlazorApp.Components.Games.Mini2PGame;

public class State : GameState<Player, Piece>
{
	public override uint Width => 20;

	public override uint Height => 30;

	public const int SpacesPerTurn = 5;

	private int _spacesMoved = 0;

	protected override Location[] GetValidMovesInner(Player player, Piece piece) =>
		piece.Location
			.GetAdjacentLocations(Directions.All, SpacesPerTurn - _spacesMoved)
			.Except(PlayerPieces[player.Name].Select(p => p.Location))
			.ToArray();

	protected override (string currentPlayer, string? logTemplate, object?[] logParams) PlayInner(Player player, Piece piece, Location location)
	{
		int distance = piece.Location.Distance(location);
		_spacesMoved += distance;

		piece.X = location.X;
		piece.Y = location.Y;

		if (_spacesMoved == SpacesPerTurn) _spacesMoved = 0;

		return (CurrentPlayer!, "{player} moved {piece} {spaces} spaces to {location}", [player, piece, distance, location]);
	}

	protected override (bool result, string? reason) ValidateInner(Player player, Piece piece, Location location)
	{
		throw new NotImplementedException();
	}
}
