using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Abstractions;

public abstract class GameState<TPlayer, TPiece>(ILogger<GameState<TPlayer, TPiece>> logger)
	where TPlayer : PlayerBase
	where TPiece : PieceBase
{
	protected readonly ILogger<GameState<TPlayer, TPiece>> Logger = logger;
	
	public abstract uint Width { get; init; }
	public abstract uint Height { get; init; }
	public string? CurrentPlayer { get; init; }
	public bool IsActive { get; init; } = true;
	public HashSet<TPlayer> Players { get; init; } = [];
	public HashSet<TPiece> Pieces { get; init; } = [];
	[JsonIgnore]
	public ILookup<string, TPiece> PlayerPieces => Pieces.ToLookup(p => p.PlayerName, StringComparer.OrdinalIgnoreCase);
	[JsonIgnore]
	public Dictionary<string, TPlayer> PlayersByName => Players.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

	public GameState<TPlayer, TPiece> GetView(string playerName)
	{
		var player = PlayersByName[playerName];
		return GetViewInner(player);
	}

	protected abstract GameState<TPlayer, TPiece> GetViewInner(TPlayer player);	

	public (bool result, string? reason) Validate(TPlayer player, TPiece piece, (int x, int y) location)
	{
		if (player.Name != CurrentPlayer) return (false, "Not your turn");
		if (location.x >= Width || location.y >= Height) return (false, "Out of bounds");
		return ValidateInner(player, piece, location);
	}

	protected abstract (bool result, string? reason) ValidateInner(TPlayer player, TPiece piece, (int x, int y) location);

	protected abstract (string? logTemplate, object?[] logParams, GameState<TPlayer, TPiece>) PlayInner(TPlayer player, TPiece piece, (int x, int y) location);

	public GameState<TPlayer, TPiece> Play(string playerName, TPiece piece, (int x, int y) location)
	{
		using var logScope = Logger.BeginScope("{Player}, {Piece}, {Location}", playerName, piece, location);

		var player = PlayersByName[playerName];
		var (valid, reason) = Validate(player, piece, location);
		if (!valid) throw new Exception("Invalid move: " + reason);				

		var (logTemplate, logParams, result) = PlayInner(player, piece, location);		

		Logger.LogInformation(logTemplate ?? "Played", logParams);

		return result;
	}

	public GameState<TPlayer, TPiece> Resign(string playerName)
	{
		var player = PlayersByName[playerName];
		Logger.LogInformation("{Player} resigns", playerName);
		player.IsActive = false;
		return this;
	}
}
