using Abstractions;
using Database;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp;

public class ApplicationEventRelay(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ApplicationEventRelay> logger) : EventRelay
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;
	private readonly ILogger<ApplicationEventRelay> _logger = logger;

	protected override Task<string[]> GetActivePlayersAsync(int instanceId)
	{
		throw new NotImplementedException();
	}
}
