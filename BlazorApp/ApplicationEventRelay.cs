using Abstractions;
using Database;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp;

public class ApplicationEventRelay(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ApplicationEventRelay> logger) : EventRelay
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;
	private readonly ILogger<ApplicationEventRelay> _logger = logger;

	protected override async Task<string[]> GetActivePlayersAsync(int instanceId)
	{
		using var db = _dbFactory.CreateDbContext();
		return (await db.GameInstancePlayers
			.Include(p => p.User)
			.Where(row => row.GameInstanceId == instanceId)
			.ToArrayAsync())
			.Select(p => p.User!.UserName!)
			.ToArray();
	}
}
