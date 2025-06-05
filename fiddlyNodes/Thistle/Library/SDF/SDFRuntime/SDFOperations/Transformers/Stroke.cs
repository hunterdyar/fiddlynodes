namespace fiddlyNodes.Thistle.Library;

public class Stroke : SDFOperationBase
{
	private float _width;
	private float _radius;
	
	public Stroke(float width) : base(null)
	{
		_width = width;
		_radius = width / 2f;
	}

	public override float Calculate(float x, float y, int a = 0, int b = 0)
	{
		return MathF.Abs(CalculateFrom(x, y, a,b)) - _radius;
	}

	public override object Clone()
	{
		var s = new Stroke(_width)
		{
			Parent = this.Parent
		};
		return s;
	}

	public void SetWidth(float width)
	{
		_width = width;
		_radius = width / 2f;
	}
}