namespace fiddlyNodes.Thistle.Library;

public class Constant : SDFOperationBase
{
	private readonly float _value;
	public Constant(float value)
	{
		_value = value;
	}

	public override float Calculate(float x, float y, int idx = 0, int idy = 0)
	{
		return _value;
	}

	public override object Clone()
	{
		return new Constant(_value);
	}
}