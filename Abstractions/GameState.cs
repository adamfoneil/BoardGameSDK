using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Abstractions;

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

	public (bool result, string? reason) Validate(TPlayer player, TPiece piece, (int x, int y) location)
	{
		if (player.Name != CurrentPlayer) return (false, "Not your turn");
		if (location.x >= Width || location.y >= Height) return (false, "Out of bounds");
		return ValidateInner(player, piece, location);
	}

	protected abstract (bool result, string? reason) ValidateInner(TPlayer player, TPiece piece, (int x, int y) location);

	protected abstract (string? logTemplate, object?[] logParams) PlayInner(TPlayer player, TPiece piece, (int x, int y) location);

	public void Play(string playerName, TPiece piece, (int x, int y) location)
	{
		var player = PlayersByName[playerName];
		var (valid, reason) = Validate(player, piece, location);
		if (!valid) throw new Exception("Invalid move: " + reason);				

		var (logTemplate, logParams) = PlayInner(player, piece, location);		
	}

	public void Resign(string playerName)
	{
		var player = PlayersByName[playerName];	
		player.IsActive = false;		
	}
}
