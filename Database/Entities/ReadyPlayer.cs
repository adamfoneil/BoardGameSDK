using Database.Entities.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

public class ReadyPlayer : BaseTable
{
	public GameType Game { get; set; }
	public int UserId { get; set; }

	public ApplicationUser? User { get; set; }
}

public class ReadyPlayerConfiguration : IEntityTypeConfiguration<ReadyPlayer>
{
	public void Configure(EntityTypeBuilder<ReadyPlayer> builder)
	{
		builder.HasAlternateKey(nameof(ReadyPlayer.Game), nameof(ReadyPlayer.UserId));

		builder
			.HasOne(rp => rp.User)
			.WithMany(u => u.ReadyToPlay)
			.HasForeignKey(rp => rp.UserId)
			.HasPrincipalKey(u => u.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}