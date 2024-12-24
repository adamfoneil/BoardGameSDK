using Database.Entities;

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
}
