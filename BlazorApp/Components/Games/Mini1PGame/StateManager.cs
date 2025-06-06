﻿using Abstractions;
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
		return await db.GetGameStateAsync<MiniGameState>(instanceId);
	}

	protected override async Task SaveInnerAsync()
	{		
		using var db = _dbFactory.CreateDbContext();
		await db.SaveStateAsync(InstanceId, State);
	}

	protected override async Task<(string Url, int InstanceId, MiniGameState State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		var state = new MiniGameState
		{
			CurrentPlayer = players.First().Name,
			Players = players.Select(p => new MiniGamePlayer()
			{
				Name = p.Name,
				IsHuman = p.IsHuman
			}).ToHashSet(),
			Pieces =
			[
				new MiniGamePiece() { X = 1, Y = 19, Name = "A", PlayerName = players.First().Name },
				new MiniGamePiece() { X = 1, Y = 20, Name = "B", PlayerName = players.First().Name },
				new MiniGamePiece() { X = 2, Y = 20, Name = "C", PlayerName = players.First().Name }
			]
		};

		using var db = _dbFactory.CreateDbContext();

		var playerAccounts = await db.BuildPlayersAsync(players);

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
