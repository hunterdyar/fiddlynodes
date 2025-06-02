using Thistle;

namespace fiddlyNodes.Thistle.Library;


public static class Primtives
{
	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Circle(TSDF sdf, TFloat radius)
	{
		var c = new Circle(radius.Value);
		sdf.AddOperation(c);
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Square(TSDF sdf, TFloat size)
	{
		var c = new Rect(size.Value);
		sdf.AddOperation(c);
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat, ThistleType.Tfloat)]
	public static void Box(TSDF sdf, TFloat width, TFloat height)
	{
		var c = new Rect(width.Value, height.Value);
		sdf.AddOperation(c);
	}
}