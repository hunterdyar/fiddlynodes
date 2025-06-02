using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class CircleNode : Node
{
	private SDFConstantProperty property;
	public CircleNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Circle";
		property = new SDFConstantProperty("Circle", this)
		{
			_operation = new Circle(100)
		};
		AddChild(property);
	}

	public override void Draw()
	{
		base.Draw();
		property.Draw();
	}
	
	
}