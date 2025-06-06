using fiddlyNodes;
using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

public class SDFCircleProperty : SDFConstantProperty
{
	public NumberProperty Radius;
	public Circle Op => _circle;
	private Circle _circle;
	public SDFCircleProperty(string propertyName, NumberProperty radiusProp, Node node) : base(propertyName, node)
	{
		Radius = radiusProp;
		_circle = new Circle(radiusProp.Value.Value);
	}
	
	public override TSDF GetValue()
	{
		//reach out to the appropriate GetValue calls so it goes up the chain.
		var radius = Radius.GetValue();
		_circle.SetRadius(radius.Value);
		return new TSDF(_circle);
	}

	public override IEnumerable<NodeProperty> GetProperties()
	{
		yield return Radius;
	}
}