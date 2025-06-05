using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class TranslateNode : Node
{
	private Vec2Property _vec2;
	private SDFPassthroughProperty _passthrough;
	private Translate _operation;
	public TranslateNode(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Translate";

		_vec2 = new Vec2Property("Offset", this, PortPosition.Input);

		_vec2.OnChange += (value) =>
		{
			_operation.SetOffset(value.Value);
		};

		_passthrough = new SDFPassthroughProperty("SDF", this);
		_operation = new Translate(_vec2.Value.Value.X, _vec2.Value.Value.Y);
		_passthrough.AddOp(_operation);

		AddProperties(_passthrough, _vec2);
	}

	public new static string DisplayName => "Translate";
	public new static string[] Aliases => ["translate", "move"];
}