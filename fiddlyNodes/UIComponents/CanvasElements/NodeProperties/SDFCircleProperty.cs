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

	public override TreeBaseObject GetValue(ThistleType wantedType)
	{
		//reach out to the appropriate GetValue calls so it goes up the chain.
		_circle.SetRadius((Radius.GetValue(ThistleType.Tfloat) as TFloat).Value);
		return circleOp;
	}
}