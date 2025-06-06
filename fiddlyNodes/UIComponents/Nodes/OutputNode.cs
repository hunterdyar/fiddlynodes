using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Nodes;

public class OutputNode : Node
{
	private SDFInputProperty sdfProp;
	public OutputNode(int x, int y, GridCanvas grid) : base(x, y, 0, 0, grid)
	{
		_title = "Output";
		sdfProp = new SDFInputProperty("Value", this);
		AddProperties(sdfProp);
		_uid = "output";
	}
	
	public TSDF GetValue() => sdfProp.GetValue();
	public new static string DisplayName => "Output";
	public new static string[] Aliases => ["output"];//no aliases means this one won't show up in search results.
}