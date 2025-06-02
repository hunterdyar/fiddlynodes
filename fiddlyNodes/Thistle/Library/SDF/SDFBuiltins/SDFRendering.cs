using static Raylib_cs.Raylib;

namespace fiddlyNodes.Thistle.Library;
//
// public static class SDFRendering
// {
// 	[BuiltIn(ThistleType.TImage, ThistleType.tsdf)]
// 	public static void RenderToImage(TImage target, TSDF sdf)
// 	{
// 		int w = target.Value.Width;
// 		int h = target.Value.Height;
//
// 		for (int y = 0; y < h; y++)
// 		{
// 			for (int x = 0; x < w; x++)
// 			{
// 				var t = sdf.GetSDFValue(x, y);
// 				var c = t > 0 ? ColorConstants.White : ColorConstants.Black;
// 				var targetValue = target.Value;
// 				ImageDrawPixel(ref targetValue, x, y, c.Value);
// 				target.Value = targetValue;
// 			}
// 		}
// 	}
//
// 	[BuiltIn(ThistleType.TImage, ThistleType.tsdf)]
// 	public static void Render( TSDF sdf)
// 	{
// 		int w = GetScreenWidth();
// 		int h = GetScreenHeight();
//
// 		for (int y = 0; y < h; y++)
// 		{
// 			for (int x = 0; x < w; x++)
// 			{
// 				var t = sdf.GetSDFValue(x, y);
// 				if(t <= 0){
// 					DrawPixel(x, y, sdf.Color.Value);
// 				}
// 			}
// 		}
// 	}
// }	