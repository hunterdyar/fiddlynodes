namespace fiddlyNodes.Thistle.Library;

public class Circle : SDFOperationBase
{
	
	private float _radius;

	public Circle(float radius)
	{
		_radius = radius;
	}

	public override float Calculate(float x, float y, int a = 0, int b=0)
	{
		float input = CalculateFrom(x, y,a,b);
		return SDFOperationBase.Min(input, SDFFormula.Circle(x, y, _radius));
	}


	public override string ToString()
	{
		return "Circle (r: " + _radius+")";
	}

	public override object Clone()
	{
		return new Circle(_radius)
		{
			Parent = Parent
		};
	}

	public void SetRadius(float radius)
	{
		_radius = radius;
	}
}