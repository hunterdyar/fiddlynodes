using fiddlyNodes.Thistle;
using Thistle.Library;

namespace fiddlyNodes.Thistle.Library;

public static class SDFTypeFunctions
{
	[StaticCall(ThistleType.tnone, ThistleType.tsdf)]
	public static TSDF New()
	{
		return new TSDF();
	}
}