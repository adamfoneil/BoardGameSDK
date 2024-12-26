﻿using Abstractions;

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

		string currentPlayer = CurrentPlayer!;

		if (_spacesMoved == SpacesPerTurn)
		{
			CurrentPlayerIndex++;
			if (CurrentPlayerIndex > Players.Count) CurrentPlayerIndex = 0;
			_spacesMoved = 0;
			currentPlayer = Players.ToArray()[CurrentPlayerIndex].Name;
		}

		return (currentPlayer, "{player} moved {piece} {spaces} spaces to {location}", [player, piece, distance, location]);
	}

	protected override (bool result, string? reason) ValidateInner(Player player, Piece piece, Location location)
	{
		// no capturing in this game, just movement
		if (PiecesByLocation.ContainsKey(location)) return (false, "Piece already there");
		return (true, default);
	}
}
