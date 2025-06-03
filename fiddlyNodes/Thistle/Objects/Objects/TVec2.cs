using System.Numerics;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;

//todo: is this 2 TFloats or a native Vec2?
public class TVec2 : TreeNativeObject<Vector2>
{
	public TVec2(Vector2 value) : base(value)
	{
	}

	public override TreeBaseObject Clone()
	{
		return new TVec2(Value);
	}
}