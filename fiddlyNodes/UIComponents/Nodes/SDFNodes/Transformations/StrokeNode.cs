using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class StrokeNode : Node
{
	private NumberProperty _stroke;
	private SDFPassthroughProperty _passthrough;
	private Stroke _operation;

	public StrokeNode(int x, int y, GridCanvas grid) : base(x, y,0, 0, grid)
	{
		_title = "Stroke";

		_stroke = new NumberProperty("Offset", this, PortPosition.Input);

		_stroke.OnChange += (value) => { _operation.SetWidth(value.Value); };

		_passthrough = new SDFPassthroughProperty("SDF", this);
		_operation = new Stroke(_stroke.Value.Value);
		_passthrough.AddOp(_operation);

		AddProperties(_passthrough, _stroke);
	}

	public new static string DisplayName => "Stroke";
	public new static string[] Aliases => ["stroke", "outline", "border"];
}