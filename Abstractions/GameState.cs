using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Abstractions;

public abstract class GameState<TPlayer, TPiece>(ILogger<GameState<TPlayer, TPiece>> logger)
	where TPlayer : PlayerBase
	where TPiece : PieceBase
{
	protected readonly ILogger<GameState<TPlayer, TPiece>> Logger = logger;
	
	public abstract int Width { get; init; }
	public abstract int Height { get; init; }
	public string? CurrentPlayer { get; init; }
	public bool IsActive { get; init; }
	public HashSet<TPlayer> Players { get; init; } = [];
	[JsonIgnore]
	public Dictionary<string, TPlayer> PlayerNames => Players.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
	public HashSet<TPiece> Pieces { get; init; } = [];
	[JsonIgnore]
	public ILookup<string, TPiece> PlayerPieces => Pieces.ToLookup(p => p.PlayerName, StringComparer.OrdinalIgnoreCase);

	public (bool result, string? reason) Validate(TPlayer player, TPiece piece, (int x, int y) location)
	{
		if (player.Name != CurrentPlayer) return (false, "Not your turn");
		if (location.x < 1 || location.x >= Width || location.y < 1 || location.y >= Height) return (false, "Out of bounds");
		return ValidateInner(player, piece, location);
	}

	protected abstract (bool result, string? reason) ValidateInner(TPlayer player, TPiece piece, (int x, int y) location);

	protected abstract (string logTemplate, object?[] logParams, GameState<TPlayer, TPiece>) PlayInner(TPlayer player, TPiece piece, (int x, int y) location);

	public GameState<TPlayer, TPiece> Play(string playerName, TPiece piece, (int x, int y) location)
	{
		var player = PlayerNames[playerName];
		var (valid, reason) = Validate(player, piece, location);
		if (!valid) throw new Exception("Invalid move: " + reason);

		Logger.LogInformation("{Player} plays {Piece} at {Location}", playerName, piece, location);

		var (template, logParams, result) = PlayInner(player, piece, location);

		Logger.LogInformation(template, logParams);

		return result;
	}

	public GameState<TPlayer, TPiece> Resign(string playerName)
	{
		var player = PlayerNames[playerName];
		Logger.LogInformation("{Player} resigns", playerName);
		player.IsActive = false;
		return this;
	}
}
