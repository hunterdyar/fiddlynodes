namespace fiddlyNodes.Thistle.Library;

using System.Numerics;
public class RotationalRepeat : SDFOperationBase
{
	private int _slices;

	public RotationalRepeat(int slices)
	{
		this._slices = slices;
	}
	
	public override float Calculate(float x, float y, int idx = 0, int idy=0)
	{
		float sp = MathF.Tau / _slices;
		float an = MathF.Atan(y/x);
		idx = (int)MathF.Floor(an / sp);

		float a1 = sp * (idx + 0.0f);
		float a2 = sp * (idx + 1.0f);

		//https://iquilezles.org/articles/sdfrepetition/
		
		float ra = MathF.Cos(a1);
		float rb = -MathF.Sin(a1);
		float rc = MathF.Sin(a1);
		float rd = MathF.Cos(a1);

		float ra2 = MathF.Cos(a2);
		float rb2 = -MathF.Sin(a2);
		float rc2 = MathF.Sin(a2);
		float rd2 = MathF.Cos(a2);
		
		
		//had to look up my matrix math for this, still got it super wrong.
		float r1x = ra*x+rb*y;
		float r1y = rc*x+rd*y;

		float r2x = ra2 * x + rb2 * y;
		float r2y = rc2 * x + rd2 * y;

		return CalculateFrom(r1x, r1y, idx, idy);
		//return MathF.Min(CalculateFrom(r1x, r1y), CalculateFrom(r2x, r2y));
	}

	public override object Clone()
	{
		return new RotationalRepeat(_slices)
		{
			Parent = Parent
		};
	}
}