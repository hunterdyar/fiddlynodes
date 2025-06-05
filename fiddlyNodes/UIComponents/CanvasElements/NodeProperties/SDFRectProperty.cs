using fiddlyNodes;
using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

public class SDFRectProperty : SDFConstantProperty
{
	public NumberProperty Width;
	public NumberProperty Height;
	public Rect Op => _rect;
	private Rect _rect;
	public SDFRectProperty(string propertyName, NumberProperty wProp, NumberProperty hProp, Node node) : base(propertyName, node)
	{
		Width = wProp;
		Height = hProp;
		_rect = new Rect(Width.Value.Value, Height.Value.Value);
	}
	
	public override TSDF GetValue()
	{
		//reach out to the appropriate GetValue calls so it goes up the chain.
		var w = Width.GetValue().Value;
		var h = Height.GetValue().Value;
		_rect.SetSize(w,h);
		return new TSDF(_rect);
	}
}