using Microsoft.Extensions.Logging;

namespace Abstractions;

public abstract class GameStateManager<TGameState, TPlayer, TPiece>(ILogger<GameStateManager<TGameState, TPlayer, TPiece>> logger)
	where TGameState : GameState<TPlayer, TPiece>
	where TPlayer : Player
	where TPiece : Piece
{
	protected readonly ILogger<GameStateManager<TGameState, TPlayer, TPiece>> Logger = logger;
	protected TGameState? State { get; private set; }
	protected int InstanceId { get; private set; }

	public abstract uint MinPlayers { get; }
	public abstract uint MaxPlayers { get; }

	public async Task<string> StartAsync(bool testMode, (string Name, bool IsHuman)[] players)
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

		var (url, instanceId, state) = await StartInnerAsync(testMode, players);

		State = state;
		InstanceId = instanceId;

		return url;
	}

	protected abstract Task<(string Url, int InstanceId, TGameState State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players);

	public async Task LoadAsync(string key)
	{
		var (instanceId, state) = await LoadInnerAsync(key);
		State = state;
		InstanceId = instanceId;
	}

	protected abstract Task<(int InstanceId, TGameState State)> LoadInnerAsync(string key);
}
