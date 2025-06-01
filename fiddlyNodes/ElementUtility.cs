using System.Numerics;
using Raylib_cs;

namespace fiddlyNodes;

public static class ElementUtility
{
	public static bool Overlaps(this Rectangle rectangle, Vector2 point)
	{
		return point.X >= rectangle.X
			&& point.X <= rectangle.X + rectangle.Width
			&& point.Y >= rectangle.Y
			&& point.Y <= rectangle.Y + rectangle.Height;
	}
	
	public static bool Overlaps(this Rectangle rectangle, Rectangle other)
	{
		return !(rectangle.X + rectangle.Width < other.X
		       && rectangle.X > other.X + other.Width
		       && rectangle.Y + rectangle.Height < other.Y
		       && rectangle.Y > other.Y + other.Height);
	
	}

	public static float DistanceToLine(Vector2 a, Vector2 b, Vector2 pos)
	{
		var num = float.Abs((b.Y - a.Y) * pos.X - (b.X - a.X) * pos.Y + b.X * a.Y - b.Y * a.X);
		var denum = float.Sqrt(float.Pow(b.Y-a.Y,2) + float.Pow(b.X-a.X,2));
		return num / denum;
	}
}