using Abstractions;
using Database;
using Database.Entities;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace BlazorApp.Components.Games.MiniGame;

public class StateManager(
	ILogger<StateManager> logger,
	Hashids hashids,
	IDbContextFactory<ApplicationDbContext> dbFactory) : GameStateManager<MiniGameState, MiniGamePlayer, MiniGamePiece>(logger)
{
	private readonly Hashids _hashids = hashids;
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

	public override uint MinPlayers => 1;

	public override uint MaxPlayers => 1;

	protected override async Task<(int InstanceId, MiniGameState State)> LoadInnerAsync(string key)
	{
		var instanceId = _hashids.DecodeSingle(key);
		using var db = _dbFactory.CreateDbContext();
		var instance = await db.GameInstances.SingleOrDefaultAsync(row => row.Id == instanceId) ?? throw new Exception("Game instance not found.");
		var state = JsonSerializer.Deserialize<MiniGameState>(instance.State) ?? throw new Exception("Could not deserialize game state.");
		return (instance.Id, state);
	}

	protected override async Task<(string Url, int InstanceId, MiniGameState State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		var state = new MiniGameState()
		{
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

		var playerAccounts = await db.Users.Join(
			players.Where(p => p.IsHuman),
			account => account.UserName, p => p.Name, (account, p) => new GameInstancePlayer() { UserId = account.UserId }, StringComparer.OrdinalIgnoreCase)
			.ToArrayAsync();

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

		var url = $"/Mini/{_hashids.Encode(instance.Id)}";
		await db.GameInstances.Where(row => row.Id == instance.Id).ExecuteUpdateAsync(row => row.SetProperty(x => x.Url, url));
		
		return (url, instance.Id, state);
	}
}
