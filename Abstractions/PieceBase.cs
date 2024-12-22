namespace Abstractions;

public class PieceBase
{
	public string PlayerName { get; init; } = default!;
	public int X { get; set; }
	public int Y { get; set; }
	public (int X, int Y) Location => (X, Y);
}
