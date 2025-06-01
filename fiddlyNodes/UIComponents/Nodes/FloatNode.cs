using fiddlyNodes.NodeElements;

namespace fiddlyNodes.Nodes;

public class FloatNode : Node
{
	public FloatNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Float";
		var propA = new StringProperty("value", this);
		AddChild(propA);
	}
}