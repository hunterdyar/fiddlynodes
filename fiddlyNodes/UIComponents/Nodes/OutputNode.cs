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
		AddProperties(sdfProp);
	}
	
	public TSDF GetValue() => sdfProp.GetValue();
}