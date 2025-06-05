using fiddlyNodes;
using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

public class SDFSquareProperty : SDFConstantProperty
{
	public NumberProperty Size;
	public Rect Op => _rect;
	private Rect _rect;
	public SDFSquareProperty(string propertyName, NumberProperty sizeProp, Node node) : base(propertyName, node)
	{
		Size = sizeProp;
		_rect = new Rect(sizeProp.Value.Value);
	}
	
	public override TSDF GetValue()
	{
		//reach out to the appropriate GetValue calls so it goes up the chain.
		var radius = Size.GetValue();
		_rect.SetSize(radius.Value);
		return new TSDF(_rect);
	}
}