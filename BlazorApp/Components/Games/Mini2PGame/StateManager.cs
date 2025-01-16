using Abstractions;
using Database;
using Database.Entities;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlazorApp.Components.Games.Mini2PGame;

public class StateManager(
	ILogger<StateManager> logger,
	Hashids hashids,
	IDbContextFactory<ApplicationDbContext> dbFactory) : GameStateManager<State, Player, Piece>(logger)
{
	private readonly Hashids _hashids = hashids;
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

	public override uint MinPlayers => 2;
	public override uint MaxPlayers => 2;

	public const int SpacesPerTurn = 5;

	private int _spacesMoved = 0;

	public int MovesRemaining => SpacesPerTurn - _spacesMoved;

	public async Task LoadAsync(string key)
	{
		var instanceId = _hashids.DecodeSingle(key);
		await LoadInstanceAsync(instanceId);
	}

	protected override async Task<State> LoadInstanceInnerAsync(int instanceId)
	{
		using var db = _dbFactory.CreateDbContext();
		return await db.GetGameStateAsync<State>(instanceId);
	}

	protected override async Task SaveInnerAsync()
	{
		using var db = _dbFactory.CreateDbContext();
		await db.SaveStateAsync(InstanceId, State);
	}

	protected override async Task<(string Url, int InstanceId, State State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		var state = new State()
		{
			CurrentPlayer = players.First().Name,
			Players = players.Select(p => new Player()
			{
				Name = p.Name,
				IsHuman = p.IsHuman
			}).ToHashSet(),
			Pieces =
			[
				new Piece() { X = 1, Y = 19, Name = "A", PlayerName = players.First().Name },
				new Piece() { X = 1, Y = 20, Name = "B", PlayerName = players.First().Name },
				new Piece() { X = 2, Y = 20, Name = "C", PlayerName = players.First().Name },
				new Piece() { X = 19, Y = 1, Name = "A", PlayerName = players.Last().Name },
				new Piece() { X = 20, Y = 1, Name = "B", PlayerName = players.Last().Name },
				new Piece() { X = 20, Y = 2, Name = "C", PlayerName = players.Last().Name }
			]
		};

		using var db = _dbFactory.CreateDbContext();
		var playerAccounts = await db.BuildPlayersAsync(players);

		var instance = new GameInstance()
		{
			Type = GameType.Mini2P,
			State = JsonSerializer.Serialize(state),
			IsTestMode = testMode,
			Url = "dummy",
			PlayerAccounts = playerAccounts
		};

		db.GameInstances.Add(instance);

		await db.SaveChangesAsync();

		var url = $"/Mini2P/{_hashids.Encode(instance.Id)}";
		await db.GameInstances.Where(row => row.Id == instance.Id).ExecuteUpdateAsync(row => row.SetProperty(x => x.Url, url));

		return (url, instance.Id, state);
	}
}
