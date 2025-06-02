using fiddlyNodes.Thistle;

namespace fiddlyNodes.Thistle.Library;

public class Transforms
{
	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Stroke(TSDF sdf, TFloat width)
	{
		var s = new Stroke(width.Value);
		sdf.AddOperation(s);
	}

	//todo: fixing hte libraryHelper to use the attributes should let us find the correct overload.
	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat, ThistleType.Tfloat)]
	public static void Translate(TSDF sdf, TFloat x, TFloat y)
	{
		var s = new Translate(x.Value, y.Value);
		sdf.AddOperation(s);
	}
	
	[BuiltIn(ThistleType.tsdf, ThistleType.tint, ThistleType.tint)]
	public static void Translate(TSDF sdf, TInt x, TInt y)
	{
		var s = new Translate(x.Value,y.Value);
		sdf.AddOperation(s);
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Rotate(TSDF sdf, TFloat angle)
	{
		sdf.AddOperation(new Rotate(angle.Value));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Scale(TSDF sdf, TFloat s)
	{
		sdf.AddOperation(new Scale(s.Value));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void MirrorX(TSDF sdf, TFloat axis)
	{
		sdf.AddOperation(new Mirror(axis.Value, true));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void MirrorY(TSDF sdf, TFloat axis)
	{
		sdf.AddOperation(new Mirror(axis.Value, false));
	}

	//square tiles.
	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Tile(TSDF sdf, TFloat gridScale)
	{
		sdf.AddOperation(new Tile(gridScale.Value));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.Tfloat)]
	public static void Round(TSDF sdf, TFloat radius)
	{
		sdf.AddOperation(new Round(radius.Value));
	}

	//square tiles.
	[BuiltIn(ThistleType.tsdf, ThistleType.tint)]
	public static void RotRep(TSDF sdf, TInt slices)
	{
		sdf.AddOperation(new RotationalRepeat(slices.Value));
	}


	[BuiltIn(ThistleType.tsdf, ThistleType.tint, ThistleType.tint, ThistleType.Tfloat, ThistleType.Tfloat)]
	public static void Array(TSDF sdf, TInt countX, TInt countY, TFloat scaleX, TFloat scaleY)
	{
		sdf.AddOperation(new Repeat(countX.Value, countY.Value, scaleX.Value, scaleY.Value));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.tint, ThistleType.tint, ThistleType.Tfloat)]
	public static void Array(TSDF sdf, TInt countX, TInt countY, TFloat scale)
	{
		sdf.AddOperation(new Repeat(countX.Value, countY.Value, scale.Value));
	}


	[BuiltIn(ThistleType.tsdf, ThistleType.tint, ThistleType.Tfloat)]
	public static void ArrayX(TSDF sdf, TInt countX, TFloat scale)
	{
		sdf.AddOperation(new Repeat(countX.Value, 1, scale.Value));
	}

	[BuiltIn(ThistleType.tsdf, ThistleType.tint,ThistleType.Tfloat)]
	public static void ArrayY(TSDF sdf, TInt countY, TFloat scale)
	{
		sdf.AddOperation(new Repeat(1,countY.Value, scale.Value));
	}
}