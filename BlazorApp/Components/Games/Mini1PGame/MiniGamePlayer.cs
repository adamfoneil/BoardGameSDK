using Abstractions;
using System.Diagnostics;

namespace BlazorApp.Components.Games.Mini1PGame;

[DebuggerDisplay("{Name}")]
public class MiniGamePlayer : Player
{
	public override string ToString() => Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => obj is MiniGamePlayer player && Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase);	
}
