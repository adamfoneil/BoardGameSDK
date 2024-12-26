using Abstractions;
using System.Diagnostics;

namespace BlazorApp.Components.Games.Mini1PGame;

[DebuggerDisplay("{Name}")]
public class MiniGamePiece : Piece
{
	public required string Name { get; init; }

	public override string ToString() => Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => obj is MiniGamePiece other && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

	#region maybe-later
	public static Dictionary<char, string[]> DefaultNames => new()
	{
		['a'] = ["Ajax", "Acrid", "Avon", "Anvil"],
		['b'] = ["Brunswick", "Banjo", "Boom", "Bronze"],
		['c'] = ["Comet", "Cathode", "Carbon", "Cathode"],
		['d'] = ["Doodle", "Damsel", "Darius", "Doofus"],
		['e'] = ["Ember", "Engine", "Euclid", "Enzo"],
		['f'] = ["Fancy", "Fidelio", "Fulsome", "Fenway"],
		['g'] = ["Garlic", "Gallium", "Galax", "Galway"]
	};
	#endregion
}
