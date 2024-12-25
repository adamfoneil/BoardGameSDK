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
	public bool IsActive { get; set; } = true;
	public HashSet<TPlayer> Players { get; init; } = [];
	public HashSet<TPiece> Pieces { get; init; } = [];
	[JsonIgnore]
	public ILookup<string, TPiece> PlayerPieces => Pieces.ToLookup(p => p.PlayerName, StringComparer.OrdinalIgnoreCase);
	[JsonIgnore]
	public Dictionary<string, TPlayer> PlayersByName => Players.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);	

	public (bool result, string? reason) Validate(TPlayer player, TPiece piece, Location location)
	{
		if (player.Name != CurrentPlayer) return (false, "Not your turn");
		if (location.X >= Width || location.Y >= Height) return (false, "Out of bounds");
		if (!GetValidMoves(player, piece).Any(l => l == location)) return (false, "Invalid move");
		return ValidateInner(player, piece, location);
	}

	public abstract Location[] GetValidMoves(TPlayer player, TPiece piece);

	protected abstract (bool result, string? reason) ValidateInner(TPlayer player, TPiece piece, Location location);

	protected abstract (string? logTemplate, object?[] logParams) PlayInner(TPlayer player, TPiece piece, Location location);

	public (string? logTemplate, object?[] logParams) Play(string playerName, TPiece piece, Location location)
	{
		var player = PlayersByName[playerName];
		var (valid, reason) = Validate(player, piece, location);
		if (!valid) throw new Exception("Invalid move: " + reason);				

		return PlayInner(player, piece, location);		
	}

	public void Resign(string playerName)
	{
		var player = PlayersByName[playerName];	
		player.IsActive = false;		
	}

	public TPiece[] GetPieces(Location location) => Pieces.Where(p => p.Location == location).ToArray() ?? [];
}
