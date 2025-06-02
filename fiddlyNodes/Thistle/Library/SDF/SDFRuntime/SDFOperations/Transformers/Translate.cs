namespace fiddlyNodes.Thistle.Library;


public class Translate : SDFOperationBase
{
	private readonly float _x;
	private readonly float _y;

	public Translate(float x, float y)
	{
		_x = x;
		_y = y;
	}
	public Translate(int x, int y)
	{
		_x = x;
		_y = y;
	}

	public override float Calculate(float x, float y, int a = 0, int b = 0)
	{
		return CalculateFrom(x - _x, y - _y, a,b);
	}

	public override object Clone()
	{
		var t = new Translate(_x, _y)
		{
			Parent = Parent
		};
		return t;
	}
}