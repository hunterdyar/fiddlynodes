using System.Numerics;
using System.Reflection.Metadata.Ecma335;

using fiddlyNodes.Thistle;

public static class SDFFormula
{
	public static float Circle(float x, float y, float radius)
	{
		return System.MathF.Sqrt(x * x + y * y) - radius;
	}

	public static float Rect(int x, int y, float width, float height)
	{
		Vector2 p = new Vector2(x, y);
		Vector2 b = new Vector2(width, height);
		Vector2 d = Vector2.Abs(p) - b;
		return Vector2.Max(d, new Vector2(0.0f)).Length() + float.Min(float.Max(d.X, d.Y), 0.0f);
	}
	
}