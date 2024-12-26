using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Database;

public partial class ApplicationDbContext
{
	public IQueryable<GameInstance> MyGames(int userId, GameType? gameType = null)
	{
		var query = GameInstances.Where(gi => gi.PlayerAccounts.Any(gip => gip.UserId == userId));

		if (gameType.HasValue)
		{
			query = query.Where(gi => gi.Type == gameType.Value);
		}

		return query;
	}

	public async Task<T> GetGameStateAsync<T>(int instanceId)
	{
		var instance = await GameInstances.SingleOrDefaultAsync(row => row.Id == instanceId) ?? throw new Exception("Game instance not found.");
		return JsonSerializer.Deserialize<T>(instance.State) ?? throw new Exception("Could not deserialize game state.");
	}
}
