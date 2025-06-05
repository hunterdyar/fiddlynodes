using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class SquareNode : Node
{
	private SDFSquareProperty sdfSquareProperty;
	private NumberProperty _radiusProp;
	public SquareNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Square";
		
		_radiusProp = new NumberProperty("Size", this, PortPosition.Input);
		
		_radiusProp.OnChange += (value) =>
		{
			sdfSquareProperty.Op.SetSize(value.Value);
		};

		sdfSquareProperty = new SDFSquareProperty("SDF", _radiusProp, this);

		AddProperties(sdfSquareProperty, _radiusProp);
	}
	
	public override void Draw()
	{
		base.Draw();
		sdfSquareProperty.Draw();
		_radiusProp.Draw();
	}
}