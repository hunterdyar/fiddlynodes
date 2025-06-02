using fiddlyNodes.NodeElements;

namespace fiddlyNodes.Nodes;

public class FloatNode : Node
{
	private NumberProperty _number;
	public FloatNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Float";
		_number = new NumberProperty("value", this, PortPosition.Output);
		AddProperties(_number);
	}
}