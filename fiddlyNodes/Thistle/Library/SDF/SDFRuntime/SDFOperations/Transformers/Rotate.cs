namespace fiddlyNodes.Thistle.Library;

public class Rotate : SDFOperationBase
{
	private readonly float _angle;
	public Rotate(float angle, bool useAngles = true)
	{
		if (useAngles)
		{
			this._angle = float.DegreesToRadians(angle);
		}
		else
		{
			//radians
			this._angle = angle;
		}
	}

	public override float Calculate(float x, float y, int a=0,int b =0)
	{
	
		float xp = x * MathF.Cos(_angle) - y * MathF.Sin(_angle);
		float yp = y * MathF.Cos(_angle) + x * MathF.Sin(_angle);
		return CalculateFrom(xp, yp, a,b);
	}

	public override object Clone()
	{
		var r = new Rotate(_angle)
		{
			Parent = this.Parent
		};
		return r;
	}
}