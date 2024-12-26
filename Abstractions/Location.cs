namespace Abstractions;

[Flags]
public enum Directions
{
	North = 1,
	South = 2,
	East = 4,
	West = 8,
	NorthEast = 16,
	NorthWest = 32,
	SouthEast = 64,
	SouthWest = 128,
	All = North | South | East | West | NorthEast | NorthWest | SouthEast | SouthWest
}

public record Location(int X, int Y);

public static class LocationExtensions
{
	public static IEnumerable<Location> GetAdjacentLocations(this Location location, Directions directions, int count) =>
		location.GetAdjacentLocations(directions, loc => loc.Distance(location) <= count);

	public static IEnumerable<Location> GetAdjacentLocations(this Location location, Directions directions, Func<Location, bool> whileCondition)
	{
		if (directions.HasFlag(Directions.North))
		{
			Location next = new(location.X, location.Y - 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X, next.Y - 1);
			}
		}

		if (directions.HasFlag(Directions.South))
		{
			Location next = new(location.X, location.Y + 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X, next.Y + 1);
			}
		}

		if (directions.HasFlag(Directions.East))
		{
			Location next = new(location.X + 1, location.Y);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X + 1, next.Y);
			}
		}

		if (directions.HasFlag(Directions.West))
		{
			Location next = new(location.X - 1, location.Y);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X - 1, next.Y);
			}
		}

		if (directions.HasFlag(Directions.NorthEast))
		{
			Location next = new(location.X + 1, location.Y - 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X + 1, next.Y - 1);
			}
		}

		if (directions.HasFlag(Directions.NorthWest))
		{
			Location next = new(location.X - 1, location.Y - 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X - 1, next.Y - 1);
			}
		}

		if (directions.HasFlag(Directions.SouthEast))
		{
			Location next = new(location.X + 1, location.Y + 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X + 1, next.Y + 1);
			}
		}

		if (directions.HasFlag(Directions.SouthWest))
		{
			Location next = new(location.X - 1, location.Y + 1);
			while (whileCondition(next))
			{
				yield return next;
				next = new(next.X - 1, next.Y + 1);
			}
		}
	}

	public static IEnumerable<Location> GetAdjacentLocations(this Location location, Directions directions, (int width, int height) boundingRectangle) =>
		location.GetAdjacentLocations(directions, loc => loc.X >= 1 && loc.Y >= 1 && loc.X <= boundingRectangle.width && loc.Y <= boundingRectangle.height);

	public static IEnumerable<Location> Clip(this IEnumerable<Location> source, uint width, uint height)
	{
		foreach (var loc in source)
		{
			if (loc.X >= 1 && loc.Y >= 1 && loc.X <= width && loc.Y <= height)
			{
				yield return loc;
			}
		}
	}

	public static int Distance(this Location start, Location end) => Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
}