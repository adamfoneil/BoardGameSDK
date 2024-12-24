using Microsoft.Extensions.Logging;

namespace Abstractions;

public abstract class GameFactory<TGameState, TPlayer, TPiece>(ILogger<GameFactory<TGameState, TPlayer, TPiece>> logger)
	where TGameState : GameState<TPlayer, TPiece>
	where TPlayer : Player
	where TPiece : Piece
{
	protected readonly ILogger<GameFactory<TGameState, TPlayer, TPiece>> Logger = logger;

	public abstract uint MinPlayers { get; }
	public abstract uint MaxPlayers { get; }

	public async Task<(string Url, TGameState State)> StartAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		if (players.Length < MinPlayers || players.Length > MaxPlayers)
		{
			throw new InvalidOperationException($"The number of players must be between {MinPlayers} and {MaxPlayers}.");
		}

		var uniquePlayers = players.Select(p => p.Name).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
		if (uniquePlayers.Length != players.Length)
		{
			throw new InvalidOperationException("Player names must be unique.");
		}

		return await StartInnerAsync(testMode, players);
	}

	protected abstract Task<(string Url, TGameState State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players);

	public abstract Task<TGameState> LoadAsync(string key);
}
