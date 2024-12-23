using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database;

public class ApplicationUser : IdentityUser
{
	public int UserId { get; set; }

	public ICollection<GameInstancePlayer> GameInstances { get; set; } = [];
	public ICollection<ReadyPlayer> ReadyToPlay { get; set; } = [];
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(e => e.UserId).ValueGeneratedOnAdd();
		builder.HasAlternateKey(u => u.UserId);
	}
}
