using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class MirrorNode : Node
{
	private NumberProperty _mirrorAround;
	private SDFPassthroughProperty _passthrough;
	private Mirror _operation;

	public MirrorNode(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Mirror";

		_mirrorAround = new NumberProperty("Around Axis", this, PortPosition.Input);

		_mirrorAround.OnChange += (value) => { _operation.SetAround(value.Value); };

		_passthrough = new SDFPassthroughProperty("SDF", this);
		_operation = new Mirror(_mirrorAround.Value.Value, true);
		_passthrough.AddOp(_operation);

		AddProperties(_passthrough, _mirrorAround);
	}

	public new static string DisplayName => "Mirror";
	public new static string[] Aliases => ["mirror"];
}