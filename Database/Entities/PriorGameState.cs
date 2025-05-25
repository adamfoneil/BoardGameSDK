using Database.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Entities;

public class PriorGameState : BaseTable
{
	public int GameInstanceId { get; set; }
	public int MoveNumber { get; set; }
	public string State { get; set; } = default!;

	public GameInstance? GameInstance { get; set; } = default!;
}

public class PriorGameStateConfiguration : IEntityTypeConfiguration<PriorGameState>
{
	public void Configure(EntityTypeBuilder<PriorGameState> builder)
	{
		builder.ToTable("PriorGameStates");
		builder.HasIndex(e => new { e.GameInstanceId, e.MoveNumber }).IsUnique();
		builder.HasOne(e => e.GameInstance).WithMany(e => e.StateHistory).HasForeignKey(e => e.GameInstanceId).OnDelete(DeleteBehavior.Cascade);
	}
}
