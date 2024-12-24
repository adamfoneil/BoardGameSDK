using Database.Entities.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

public class GameInstancePlayer : BaseTable
{
	public int GameInstanceId { get; set; }
	public int UserId { get; set; }
	/// <summary>
	/// extracted from GameState.Player.IsResigned
	/// </summary>
	public bool IsResigned { get; set; }

	public GameInstance? GameInstance { get; set; }
	public ApplicationUser? User { get; set; }
}

public class PlayerConfiguration : IEntityTypeConfiguration<GameInstancePlayer>
{
	public void Configure(EntityTypeBuilder<GameInstancePlayer> builder)
	{
		builder.HasAlternateKey(nameof(GameInstancePlayer.GameInstanceId), nameof(GameInstancePlayer.UserId));

		builder
			.HasOne(gip => gip.User)
			.WithMany(u => u.GameInstances)
			.HasForeignKey(gip => gip.UserId)
			.HasPrincipalKey(u => u.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}