using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class CircleNode : Node
{
	private SDFCircleProperty sdfCircleProperty;
	private NumberProperty _radiusProp;
	public CircleNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Circle";
		
		_radiusProp = new NumberProperty("Radius", this, PortPosition.Input);
		
		_radiusProp.OnChange += (value) =>
		{
			sdfCircleProperty.Op.SetRadius(value.Value);
		};

		sdfCircleProperty = new SDFCircleProperty("SDF", _radiusProp, this);

		AddProperties(sdfCircleProperty, _radiusProp);
	}
	
	public override void Draw()
	{
		base.Draw();
		sdfCircleProperty.Draw();
		_radiusProp.Draw();
	}
}