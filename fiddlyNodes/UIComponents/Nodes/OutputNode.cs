using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class OutputNode : Node
{
	private SDFInputProperty sdfProp;
	public OutputNode(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Output";
		sdfProp = new SDFInputProperty("Value", this);
		AddChild(sdfProp);
	}

	//todo: renderingContext instead of width/height to hold all the relevant information.
	public TreeBaseObject GetValue(int width, int height)
	{
		//this property should get values from what it's connected from as SDFOperations or SDFPrimitives, and smin them.
		//those properties will recursively walk as needed.
		var sdf = sdfProp.GetValue(ThistleType.tsdf);
		// sdf.AddOperation(new Circle(50));
		// sdf.AddOperation(new Translate(width / 2, height / 2));
		return sdf;
	}
}