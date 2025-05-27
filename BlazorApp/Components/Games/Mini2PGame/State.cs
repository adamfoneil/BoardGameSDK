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
	
	// Track the current challenge
	public Challenge? CurrentChallenge { get; private set; }

	protected override Location[] GetValidMovesInner(Player player, Piece piece) =>
		[.. piece.Location
			.GetAdjacentLocations(Directions.All, SpacesPerTurn - _spacesMoved)
			.Except(PlayerPieces[player.Name].Select(p => p.Location))];

	protected override string PlayInner(Player player, Piece piece, Location location, Piece? attackedPiece, Location priorLocation)
	{
		int distance = piece.Location.Distance(location);
		_spacesMoved += distance;

		piece.X = location.X;
		piece.Y = location.Y;

		string currentPlayer = CurrentPlayer!;
		
		if (_spacesMoved == SpacesPerTurn)
		{
			CurrentPlayerIndex++;
			if (CurrentPlayerIndex >= Players.Count) CurrentPlayerIndex = 0;
			_spacesMoved = 0;
			currentPlayer = Players.ToArray()[CurrentPlayerIndex].Name;
		}

		if (attackedPiece is not null && CurrentChallenge?.IsResolved == true && 
			CurrentChallenge.Defender.piece == attackedPiece)
		{
			// If this is a resolved challenge, handle according to the challenge result
			if (CurrentChallenge.AttackerWon == true)
			{
				// Attacker won, defender piece is captured
				attackedPiece.IsActive = false;
				Logger?.LogDebug("{player}'s piece {piece} captured {defender}'s piece {defenderPiece}", 
					player.Name, piece.Name, attackedPiece.PlayerName, attackedPiece.Name);
			}
			else
			{
				// Defender won, attacker retreats to original position
				piece.X = CurrentChallenge.SourceLocation.X;
				piece.Y = CurrentChallenge.SourceLocation.Y;
				Logger?.LogDebug("{player}'s piece {piece} retreated from {defender}'s piece {defenderPiece}", 
					player.Name, piece.Name, attackedPiece.PlayerName, attackedPiece.Name);
			}
			
			// Clear the current challenge
			CurrentChallenge = null;
		}
		else if (attackedPiece is not null && CurrentChallenge == null)
		{
			// If we have an attack but no challenge resolution yet, move the piece back
			// This shouldn't happen normally since we create challenges before PlayInner is called
			piece.X = priorLocation.X;
			piece.Y = priorLocation.Y;
		}

		Logger?.LogDebug("{player} moved {piece} {spaces} spaces to {location}", player, piece, distance, location);
		return currentPlayer;
	}

	// Create a new challenge between pieces
	public Challenge CreateChallenge(string attackerPlayerName, Piece attackerPiece, Piece defenderPiece, Location targetLocation, Location sourceLocation)
	{
		var attackerPlayer = PlayersByName[attackerPlayerName];
		var defenderPlayer = PlayersByName[defenderPiece.PlayerName];

		// Create and store the challenge
		CurrentChallenge = new Challenge
		{
			Attacker = (attackerPlayer, attackerPiece),
			Defender = (defenderPlayer, defenderPiece),
			TargetLocation = targetLocation,
			SourceLocation = sourceLocation,
			IsResolved = false,
			AttackerWon = null
		};

		Logger?.LogDebug("Challenge created: {attacker}'s piece {attackerPiece} vs {defender}'s piece {defenderPiece}", 
			attackerPlayer.Name, attackerPiece.Name, defenderPlayer.Name, defenderPiece.Name);

		return CurrentChallenge;
	}

	// Resolve a challenge and return the next player's name
	public string ResolveChallenge(Challenge challenge, bool attackerWins)
	{
		if (challenge != CurrentChallenge)
			throw new InvalidOperationException("Challenge to resolve doesn't match current challenge");

		// Mark the challenge as resolved with the result
		CurrentChallenge.IsResolved = true;
		CurrentChallenge.AttackerWon = attackerWins;

		Logger?.LogDebug("Challenge resolved: {winner} won", attackerWins ? CurrentChallenge.Attacker.player.Name : CurrentChallenge.Defender.player.Name);

		// Apply the result by calling Play with the same parameters as before
		// This will execute PlayInner which now handles resolved challenges
		return Play(
			CurrentChallenge.Attacker.player.Name,
			CurrentChallenge.Attacker.piece,
			CurrentChallenge.TargetLocation,
			CurrentChallenge.SourceLocation
		);
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
		public Location TargetLocation { get; set; }
		public Location SourceLocation { get; set; }
		public bool IsResolved { get; set; }
		public bool? AttackerWon { get; set; }
	}
}
