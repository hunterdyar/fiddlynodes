using Raylib_cs;

namespace fiddlyNodes;

public class GridCanvasElement : Element
{
	protected GridCanvas _grid;
	public GridCanvasElement(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height)
	{
		_grid = grid;
	}
}