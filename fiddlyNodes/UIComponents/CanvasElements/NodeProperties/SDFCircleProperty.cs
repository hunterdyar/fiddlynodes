using fiddlyNodes;
using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

public class SDFCircleProperty : SDFConstantProperty
{
	public NumberProperty Radius;
	private Circle _circle;
	private TSDFOperation circleOp;
	public SDFCircleProperty(string propertyName, NumberProperty radiusProp, Node node) : base(propertyName, node)
	{
		Radius = radiusProp;
		_circle = new Circle(radiusProp.Value.Value);
		circleOp = new TSDFOperation(_circle);
		
	}

	public override TSDFOperation GetValue()
	{
		//reach out to the appropriate GetValue calls so it goes up the chain.
		var radius = Radius.GetValue();
		_circle.SetRadius(radius.Value);
		return circleOp;
	}
}