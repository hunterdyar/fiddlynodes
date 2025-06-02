using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes;

public class TSDFOperation : TreeNativeObject<SDFOperationBase>
{
	public TSDFOperation(SDFOperationBase value) : base(value)
	{
		
	}

	public override TreeBaseObject Clone()
	{
		return new TSDFOperation(this.Value);
	}
}