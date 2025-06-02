using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class SDFConstantProperty : NodeProperty
{
	public SDFOperationBase Operation => _operation;
	public SDFOperationBase _operation;
	
	public SDFConstantProperty(string propertyName, Node node) : base(propertyName, node)
	{
		AddAndSetPort(new Port(this, PortPosition.Output));	
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
		//todo: 
		DrawPropertyName();
		base.Draw();
	}
	
}