using System.Numerics;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class SDFConstantProperty : NodeProperty
{
	public SDFOperationBase Operation => _operation;
	public SDFOperationBase _operation;
	private Label _label;
	public SDFConstantProperty(string propertyName, Node node) : base(propertyName, node)
	{
		_label = new Label("SDF", TextPosition.Right);
		AddChild(_label);
		AddAndSetPort(new Port(this, PortPosition.Output));	
	}

	public override void Recalculate()
	{
		_label.Transform.LocalPosition = Vector2.Zero;
		_label.Transform.Size = new Vector2(_transform.Size.X, _transform.Size.Y);
		base.Recalculate();
	}

	public override TreeBaseObject GetValue(ThistleType wantedType)
	{
		if (wantedType != ThistleType.tsdfOp)
		{
			throw new Exception("invalid cast. sdfconstant can only give sdfOps type.");
		}
		return new TSDFOperation(_operation);
	}

	public override void Draw()
	{
		_label.Draw();
		base.Draw();
	}
	
}