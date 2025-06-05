using fiddlyNodes.NodeElements;

namespace fiddlyNodes.Nodes;

public class FloatNode : Node
{
	private NumberProperty _number;
	public FloatNode(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Float";
		_number = new NumberProperty("value", this, PortPosition.Output);
		AddProperties(_number);
	}

	public new static string DisplayName => "Float";
	public new static string[] Aliases => ["float"];
}