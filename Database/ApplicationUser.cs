using AuthExtensions;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;

namespace Database;

public class ApplicationUser : IdentityUser, IClaimData
{
	public int UserId { get; set; }

	public ICollection<GameInstancePlayer> GameInstances { get; set; } = [];
	public ICollection<ReadyPlayer> ReadyToPlay { get; set; } = [];

	public void FromClaims(IEnumerable<Claim> claims)
	{
		UserId = int.Parse(claims.GetClaimValue(nameof(UserId)) ?? "0");
	}

	public IEnumerable<Claim> ToClaims()
	{
		yield return new Claim(nameof(UserId), UserId.ToString() ?? "0");
	}
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(e => e.UserId).ValueGeneratedOnAdd();
		builder.HasAlternateKey(u => u.UserId);
	}
}
