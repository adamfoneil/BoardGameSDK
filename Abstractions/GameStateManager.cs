using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Abstractions;

/// <summary>
/// creates, stores and accesses the state of a game from a data store
/// </summary>
public abstract class GameStateManager<TGameState, TPlayer, TPiece>(
	ILogger<GameStateManager<TGameState, TPlayer, TPiece>> logger)
	where TGameState : GameState<TPlayer, TPiece>
	where TPlayer : Player
	where TPiece : Piece
{
	protected readonly ILogger<GameStateManager<TGameState, TPlayer, TPiece>> Logger = logger;
	public TGameState? State { get; private set; }
	public int InstanceId { get; private set; }

	public bool IsLoaded => State is not null;
	public string? CurrentPlayer  { get => State?.CurrentPlayer; set => State!.CurrentPlayer = value; }

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

	public async Task LoadInstanceAsync(int instanceId)
	{		
		State = await LoadInstanceInnerAsync(instanceId);
		InstanceId = instanceId;
	}

	protected abstract Task<TGameState> LoadInstanceInnerAsync(int instanceId);

	public async Task PlayAsync(string playerName, TPiece piece, Location location)
	{
		if (State is null) throw new InvalidOperationException("Game not started.");
		if (!State.IsActive) throw new InvalidOperationException("Game is not active.");
		if (State.PlayersByName[playerName].IsActive == false) throw new InvalidOperationException("Player is not active.");
		var (logTemplate, logParams) = State.Play(playerName, piece, location);
		await SaveInnerAsync();
	}

	public async Task SaveAsync()
	{
		if (State is null) throw new InvalidOperationException("Game not started.");
		await SaveInnerAsync();
	}

	protected abstract Task SaveInnerAsync();

	public void Update(string json)
	{
		State = JsonSerializer.Deserialize<TGameState>(json) ?? throw new InvalidOperationException("Could not deserialize game state.");
	}
}
