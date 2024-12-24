using System.Text.Json.Serialization;

namespace Abstractions;

public class Piece
{
	public string PlayerName { get; init; } = default!;
	public int X { get; set; }
	public int Y { get; set; }
	[JsonIgnore]
	public (int X, int Y) Location => (X, Y);
}
