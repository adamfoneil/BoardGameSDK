using Abstractions;
using Database;
using HashidsNet;
using Microsoft.EntityFrameworkCore;

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

	protected override async Task<State> LoadInstanceInnerAsync(int instanceId)
	{
		using var db = _dbFactory.CreateDbContext();
		return await db.GetGameStateAsync<State>(instanceId);
	}

	protected override Task SaveInnerAsync()
	{
		throw new NotImplementedException();
	}

	protected override Task<(string Url, int InstanceId, State State)> StartInnerAsync(bool testMode, (string Name, bool IsHuman)[] players)
	{
		throw new NotImplementedException();
	}
}
