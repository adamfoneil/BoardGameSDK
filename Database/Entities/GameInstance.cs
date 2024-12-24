using Database.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Entities;

public enum GameType
{
	Mini1P,
	Mini2P,	
	Scom
}

public class GameInstance : BaseTable
{
	public GameType Type { get; set; }
	/// <summary>
	/// json game state
	/// </summary>
	public string State { get; set; } = default!;
	/// <summary>
	/// extracted from GameState.IsActive
	/// </summary>
	public bool IsActive { get; set; } = true;
	/// <summary>
	/// all players can see game state
	/// </summary>
	public bool IsTestMode { get; set; }
	public string Url { get; set; } = default!;

	/// <summary>
	/// human players only
	/// </summary>
	public ICollection<GameInstancePlayer> PlayerAccounts { get; set; } = [];
}

public class GameInstanceConfiguration : IEntityTypeConfiguration<GameInstance>
{
	public void Configure(EntityTypeBuilder<GameInstance> builder)
	{
		builder.Property(gi => gi.Url).HasMaxLength(100).IsRequired();
	}
}
