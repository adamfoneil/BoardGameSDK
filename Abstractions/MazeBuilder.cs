
namespace Abstractions;

public class Maze
{
	public (uint Width, uint Height) Size { get; init; }
	public (uint X, uint Y)[][] Routes { get; init; } = [];
}

public record MazeOptions
{
	public (uint Width, uint Height) Size { get; init; }
	public uint RouteCount { get; init; }
	public uint MinSegmentLength { get; init; }
	public (uint Min, uint Max) CorridorWidth { get; init; }	
}

public static class MazeBuilder
{
	public static Maze Create(MazeOptions options) => new()
	{
		Size = options.Size,
		Routes = Enumerable.Range(0, (int)options.RouteCount).Select(_ => CreateRoute(options)).ToArray()
	};

	private static (uint X, uint Y)[] CreateRoute(MazeOptions options)
	{
		var random = new Random();
		var route = new List<(uint X, uint Y)>();
		var currentX = (uint)random.Next((int)options.Size.Width);
		var currentY = (uint)random.Next((int)options.Size.Height);

		for (int i = 0; i < options.MinSegmentLength; i++)
		{
			route.Add((currentX, currentY));
			currentX = (uint)random.Next((int)options.Size.Width);
			currentY = (uint)random.Next((int)options.Size.Height);
		}

		return [.. route];
	}
}
