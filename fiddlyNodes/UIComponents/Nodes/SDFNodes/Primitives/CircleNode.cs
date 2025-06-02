using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class CircleNode : Node
{
	private SDFConstantProperty sdfOutputProp;
	private NumberProperty _radiusProp;
	public CircleNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Circle";
		
		_radiusProp = new NumberProperty("Radius", this);
		_radiusProp.OnChange += (value) =>
		{
			if (sdfOutputProp._operation is Circle circle)
			{
				circle.SetRadius(value.Value);
			}
		};
		
		sdfOutputProp = new SDFCircleProperty("SDF", _radiusProp, this)
		{
			_operation = new Circle(100),
		};
		
		AddChild(sdfOutputProp);
		AddChild(_radiusProp);
	}
	
	public override void Draw()
	{
		base.Draw();
		sdfOutputProp.Draw();
		_radiusProp.Draw();
	}
}