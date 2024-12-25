namespace Abstractions;

[Flags]
public enum Directions
{
	North,
	South,
	East,
	West,
	NorthEast,
	NorthWest,
	SouthEast,
	SouthWest
}

public record Location(int X, int Y);

public static class LocationExtensions
{
	public static IEnumerable<Location> GetAdjacentLocations(this Location location, Directions directions, int count)
	{
		if (directions.HasFlag(Directions.North))
		{
			yield return new(location.X, location.Y - count);
		}

		if (directions.HasFlag(Directions.South))
		{
			yield return new(location.X, location.Y + count);
		}

		if (directions.HasFlag(Directions.East))
		{
			yield return new(location.X + count, location.Y);
		}

		if (directions.HasFlag(Directions.West))
		{
			yield return new(location.X - count, location.Y);
		}

		if (directions.HasFlag(Directions.NorthEast))
		{
			yield return new(location.X + count, location.Y - count);
		}

		if (directions.HasFlag(Directions.NorthWest))
		{
			yield return new(location.X - count, location.Y - count);
		}

		if (directions.HasFlag(Directions.SouthEast))
		{
			yield return new(location.X + count, location.Y + count);
		}

		if (directions.HasFlag(Directions.SouthWest))
		{
			yield return new(location.X - count, location.Y + count);
		}
	}

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
				next = new(next.X + 1, next.Y);
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
}