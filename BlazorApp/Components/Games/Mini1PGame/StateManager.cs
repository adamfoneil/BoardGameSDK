using Abstractions;
using Database;
using Database.Entities;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlazorApp.Components.Games.Mini1PGame;

public class StateManager(
	ILogger<StateManager> logger,
	Hashids hashids,
	IDbContextFactory<ApplicationDbContext> dbFactory) : GameStateManager<MiniGameState, MiniGamePlayer, MiniGamePiece>(logger)
{
	private readonly Hashids _hashids = hashids;
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

	public override uint MinPlayers => 1;

	public override uint MaxPlayers => 1;

	public async Task LoadAsync(string key)
	{
		var instanceId = _hashids.DecodeSingle(key);
		await LoadInstanceAsync(instanceId);
	}

	protected override async Task<MiniGameState> LoadInstanceInnerAsync(int instanceId)
	{		
		using var db = _dbFactory.CreateDbContext();
		var instance = await db.GameInstances.SingleOrDefaultAsync(row => row.Id == instanceId) ?? throw new Exception("Game instance not found.");
		var state = JsonSerializer.Deserialize<MiniGameState>(instance.State) ?? throw new Exception("Could not deserialize game state.");
		return state;
	}

	protected override async Task SaveGameStateAsync()
	{		
		var json = JsonSerializer.Serialize(State);

		using var db = _dbFactory.CreateDbContext();

		var count = await db.GameInstances
			.Where(row => row.Id == InstanceId)
			.ExecuteUpdateAsync(row => row
				.SetProperty(x => x.State, json)
				.SetProperty(x => x.ModifiedAt, DateTime.UtcNow)
				.SetProperty(x => x.ModifiedBy, nameof(StateManager)));

		if (count != 1)
		{
			Logger.LogError("Game instance {id} not updated. Expected 1 row updated, got {count}", InstanceId, count);
			throw new Exception($"Game instance {InstanceId} not updated. Expected 1 row updated, got {count}");
		}
	}

	protected override async Task<(string Url, int InstanceId, MiniGameState State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		var state = new MiniGameState()
		{
			CurrentPlayer = players.First().Name,
			Players = players.Select(p => new MiniGamePlayer()
			{
				Name = p.Name,
				IsHuman = p.IsHuman
			}).ToHashSet(),
			Pieces =
			[
				new MiniGamePiece() { X = 1, Y = 19 },
				new MiniGamePiece() { X = 1, Y = 20 },
				new MiniGamePiece() { X = 2, Y = 20 }
			]
		};

		using var db = _dbFactory.CreateDbContext();

		var humanPlayers = players.Where(p => p.IsHuman).Select(p => p.Name).ToHashSet();

		var playerAccounts = (await db.Users.ToArrayAsync())
			.Where(acct => humanPlayers.Contains(acct.UserName, StringComparer.OrdinalIgnoreCase))
			.Select(acct => new GameInstancePlayer() { UserId = acct.UserId })
			.ToArray();

		var instance = new GameInstance()
		{
			Type = GameType.Mini1P,
			State = JsonSerializer.Serialize(state),
			IsTestMode = testMode,
			Url = "dummy",
			PlayerAccounts = playerAccounts
		};

		db.GameInstances.Add(instance);

		await db.SaveChangesAsync();

		var url = $"/Mini1P/{_hashids.Encode(instance.Id)}";
		await db.GameInstances.Where(row => row.Id == instance.Id).ExecuteUpdateAsync(row => row.SetProperty(x => x.Url, url));
		
		return (url, instance.Id, state);
	}
}
