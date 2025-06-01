using System.Numerics;
using Raylib_cs;

namespace fiddlyNodes.UIDraw;

public static class UIDrawHelpers
{
	public static void DrawWire(Vector2 from, Vector2 to, float thick, Color color)
	{
		Raylib.DrawLineEx(from,to, thick, color);
	}
}