namespace fiddlyNodes.Thistle.Library;

public class Mirror : SDFOperationBase
{
	private float _around;
	private bool _horizontal;
	// todo: control if it's the positive or negative side mirroring to the other;
	public Mirror(float around, bool horizontal)
	{
		this._around = around;
		this._horizontal = horizontal;
	}

	public override float Calculate(float x, float y, int a = 0,int b =0)
	{
		if (_horizontal)
		{
			var x2 = MathF.Abs(x - _around) + _around;
			return CalculateFrom(x2, y, a,b);
		}
		else
		{
			var y2 = MathF.Abs(y - _around) + _around;
			return CalculateFrom(x, y2, a,b);
		}
	}

	public override object Clone()
	{
		return new Mirror(_around, _horizontal)
		{
			Parent = Parent
		};
	}

	public void SetAround(float value)
	{
		_around = value;
	}

	public void SetIsHorizontal(bool value)
	{
		_horizontal = value;
	}
}