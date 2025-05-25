using System.Text.Json.Serialization;

namespace Abstractions;

/// <summary>
/// the structure and rules of a game
/// </summary>
public abstract class GameState<TPlayer, TPiece>
	where TPlayer : Player
	where TPiece : Piece
{
	public abstract uint Width { get; }
	public abstract uint Height { get; }
	public string? CurrentPlayer { get; set; }
	public int CurrentPlayerIndex { get; set; }
	public bool IsActive { get; set; } = true;
	public HashSet<TPlayer> Players { get; init; } = [];
	public HashSet<TPiece> Pieces { get; init; } = [];
	[JsonIgnore]
	public ILookup<string, TPiece> PlayerPieces => Pieces.ToLookup(p => p.PlayerName, StringComparer.OrdinalIgnoreCase);
	[JsonIgnore]
	public Dictionary<string, TPlayer> PlayersByName => Players.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
	[JsonIgnore]
	public Dictionary<Location, TPiece> PiecesByLocation => Pieces.ToDictionary(p => p.Location);
	[JsonIgnore]
	public bool PlayerChanged { get; private set; }

	public (bool result, string? reason) Validate(TPlayer player, TPiece piece, Location location)
	{
		if (player.Name != CurrentPlayer) return (false, "Not your turn");
		if (location.X > Width || location.Y > Height) return (false, "Out of bounds");
		if (!GetValidMovesInner(player, piece).Any(l => l == location)) return (false, "Invalid move");
		return ValidateInner(player, piece, location);
	}

	protected abstract Location[] GetValidMovesInner(TPlayer player, TPiece piece);

	public HashSet<Location> GetValidMoves(string playerName, TPiece piece)
	{
		var player = PlayersByName[playerName];
		return [.. GetValidMovesInner(player, piece).Clip(Width, Height)];
	}

	protected abstract (bool result, string? reason) ValidateInner(TPlayer player, TPiece piece, Location location);

	protected abstract (string currentPlayer, string? logTemplate, object?[] logParams) PlayInner(TPlayer player, TPiece piece, Location location);

	public (string currentPlayer, string? logTemplate, object?[] logParams) Play(string playerName, TPiece piece, Location location)
	{
		var player = PlayersByName[playerName];
		var (valid, reason) = Validate(player, piece, location);
		if (!valid) throw new Exception("Invalid move: " + reason);

		var (currentPlayer, logTemplate, logParams) = PlayInner(player, piece, location);

		PlayerChanged = false;

		if (currentPlayer != CurrentPlayer)
		{
			CurrentPlayer = currentPlayer;
			PlayerChanged = true;
		}

		return (currentPlayer, logTemplate, logParams);
	}

	public void Resign(string playerName)
	{
		var player = PlayersByName[playerName];	
		player.IsActive = false;		
	}

	public TPiece? GetPiece(Location location) => PiecesByLocation.TryGetValue(location, out var piece) ? piece : null;
}
