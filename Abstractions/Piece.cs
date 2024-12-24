using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace Abstractions;

public class Piece
{
	public string PlayerName { get; init; } = default!;
	public int X { get; set; }
	public int Y { get; set; }
	[JsonIgnore]
	public Location Location => new(X, Y);
}
