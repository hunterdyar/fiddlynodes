namespace fiddlyNodes.Thistle.Library;

public class Round : SDFOperationBase
{
	private float _radius;

	public Round(float radius)
	{
		_radius = radius;
	}
	

	public override float Calculate(float x, float y, int a,int b)
	{
		return CalculateFrom(x, y, a,b) - _radius;
	}

	public override object Clone()
	{
		return new Round(_radius)
		{
			Parent = Parent
		};
	}
}