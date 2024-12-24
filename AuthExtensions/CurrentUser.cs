using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthExtensions;

public class CurrentUser<TDbContext, TUser>(
	AuthenticationStateProvider authStateProvider,
	IDbContextFactory<TDbContext> dbFactory)
	where TDbContext : IdentityDbContext<TUser>
	where TUser : IdentityUser
{
	private readonly AuthenticationStateProvider _authState = authStateProvider;
	private readonly IDbContextFactory<TDbContext> _dbFactory = dbFactory;

	private TUser? _currentUser;

	public async Task<TUser?> GetCurrentUserAsync()
	{
		if (_currentUser != null)
		{
			return _currentUser;
		}

		var authState = await _authState.GetAuthenticationStateAsync();
		var user = authState.User;

		if (user.Identity is not null && user.Identity.IsAuthenticated)
		{
			using var db = _dbFactory.CreateDbContext();
			_currentUser = await db.Users.FirstOrDefaultAsync(u => u.UserName == user.Identity.Name);
		}

		return _currentUser;
	}
}