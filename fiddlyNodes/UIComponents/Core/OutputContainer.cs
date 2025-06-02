using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class OutputContainer : Element
{
	public int OutputWidth => outputWidth;
	private int outputWidth;
	public int OutputHeight => outputHeight;
	private int outputHeight;
	
	private Color _backgroundColor = Color.Beige;
	public OutputContainer(int x, int y, int width, int height) : base(x, y, width, height)
	{
		outputWidth = width;
		outputHeight = height;
		
	}

	public override void Draw()
	{
		int w = outputWidth;
		int h = outputHeight;

		var ox = (int)Transform.WorldPosition.X;
		var oy = (int)Transform.WorldPosition.Y;

		var sdf = Program.PrimaryOutputNode.GetValue(w, h) as TSDF;
		
		//todo: make parallel
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				var t = sdf.GetSDFValue(x, y);
				var color = t <= 0 ? sdf.Color.Value : _backgroundColor;
				Raylib.DrawPixel(ox+x, oy+y, color);
			}
		}
	}
}