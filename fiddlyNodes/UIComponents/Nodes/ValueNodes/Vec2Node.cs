using fiddlyNodes.NodeElements;

namespace fiddlyNodes.Nodes;

public class Vec2Node : Node
{
	private Vec2Property _v;
	public Vec2Node(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Vec 2";
		_v = new Vec2Property("Vec2", this, PortPosition.Output);
		AddProperties(_v);
	}
}