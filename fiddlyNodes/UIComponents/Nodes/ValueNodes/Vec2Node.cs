using fiddlyNodes.NodeElements;

namespace fiddlyNodes.Nodes;

public class Vec2Node : Node
{
	private Vec2Property _v;
	public Vec2Node(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Vec 2";
		_v = new Vec2Property("Vec2", this, PortPosition.Output);
		AddProperties(_v);
	}

	public new static string DisplayName => "Vec 2";
	public new static string[] Aliases => ["vec2", "vector2", "v2"];
}