using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class RectNode : Node
{
	private SDFRectProperty _sdfRectProperty;
	private NumberProperty _widthProp;
	private NumberProperty _heightProp;
	public RectNode(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Square";

		_widthProp = new NumberProperty("Width", this, PortPosition.Input);
		_heightProp = new NumberProperty("Height", this, PortPosition.Input);
		
		_heightProp.OnChange += (value) => { _sdfRectProperty.Op.SetHeight(value.Value); };
		_widthProp.OnChange += (value) => { _sdfRectProperty.Op.SetWidth(value.Value); };

		_sdfRectProperty = new SDFRectProperty("SDF", _widthProp, _heightProp, this);

		AddProperties(_sdfRectProperty, _widthProp, _heightProp);
	}

	public new static string DisplayName => "Rectangle";
	public new static string[] Aliases => ["rect", "rectangle", "box", "square"];

	public override void Draw()
	{
		base.Draw();
		_sdfRectProperty.Draw();
		_widthProp.Draw();
		_heightProp.Draw();
	}
}