using Abstractions;

namespace BlazorApp.Components.Games.Mini2PGame;

public enum Mode
{
	Moving,
	Challenging
}

public class State : GameState<Player, Piece>
{	
	public override uint Width => 20;

	public override uint Height => 20;

	public const int SpacesPerTurn = 5;

	private int _spacesMoved = 0;

	protected override Location[] GetValidMovesInner(Player player, Piece piece) =>
		[.. piece.Location
			.GetAdjacentLocations(Directions.All, SpacesPerTurn - _spacesMoved)
			.Except(PlayerPieces[player.Name].Select(p => p.Location))];

	protected override string PlayInner(Player player, Piece piece, Location location)
	{
		int distance = piece.Location.Distance(location);
		_spacesMoved += distance;

		piece.X = location.X;
		piece.Y = location.Y;

		string currentPlayer = CurrentPlayer!;
		
		if (IsChallenging(player, piece, location, out var challenge))
		{
			// save to state, change game mode to Challenge
			// todo: make it so the the piece location doesn't take effect until challenge is resolved
		}

		if (_spacesMoved == SpacesPerTurn)
		{
			CurrentPlayerIndex++;
			if (CurrentPlayerIndex >= Players.Count) CurrentPlayerIndex = 0;
			_spacesMoved = 0;
			currentPlayer = Players.ToArray()[CurrentPlayerIndex].Name;
		}

		Logger?.LogDebug("{player} moved {piece} {spaces} spaces to {location}", player, piece, distance, location);
		return currentPlayer;
	}

	private bool IsChallenging(Player player, Piece piece, Location location, out Challenge? challenge)
	{
		challenge = default;
		return false;
	}

	protected override (bool result, string? reason) ValidateInner(Player player, Piece piece, Location location)
	{
		// no capturing in this game, just movement
		//if (PiecesByLocation.ContainsKey(location)) return (false, "Piece already there");

		if (PlayerPieces[player.Name].Any(p => p.Location == location))
		{
			return (false, "Can't challenge your own pieces");
		}

		return (true, default);
	}

	/// <summary>
	/// represents one piece attacking another, tracking the attacker and defender
	/// </summary>
	public class Challenge
	{		
		public (Player player, Piece piece) Attacker { get; set; }
		public (Player player, Piece piece) Defender { get; set; }
	}
}
