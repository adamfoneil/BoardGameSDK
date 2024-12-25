namespace Abstractions;

public class StateChangeEventArgs(string toPlayer, int instanceId, string json) : EventArgs
{
	public string ToPlayer { get; } = toPlayer;
	public int InstanceId { get; } = instanceId;
	public string Json { get; } = json;
}

public delegate void StateChangeEventHandler(object sender, StateChangeEventArgs args);

/// <summary>
/// use as singleton to send and receive events from the game app
/// </summary>
public abstract class EventRelay
{
	/// <summary>
	/// handlers should check the player and instanceId to see if they should handle the events
	/// </summary>
	public event StateChangeEventHandler? StateChanged;

	/// <summary>
	/// notify active players of a game (except the event initiator) that the state of a game instance has changed
	/// </summary>
	public async Task StateChangedAsync(string fromPlayer, int instanceId, string json)
	{
		var players = await GetActivePlayersAsync(instanceId);
		foreach (var player in players.Except([fromPlayer], StringComparer.OrdinalIgnoreCase))
		{			
			StateChanged?.Invoke(this, new(player, instanceId, json));			
		}
	}

	protected abstract Task<string[]> GetActivePlayersAsync(int instanceId);

	public async Task NotifyGameStarted(int gameTypeId, string url)
	{

	}

}
