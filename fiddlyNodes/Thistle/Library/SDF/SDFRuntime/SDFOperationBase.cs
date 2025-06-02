namespace fiddlyNodes.Thistle.Library;

//todo: struct and interface?
public abstract class SDFOperationBase : ICloneable
{
	public SDFOperationBase? Parent;

	public SDFOperationBase()
	{
		Parent = null;
	}
	public SDFOperationBase(SDFOperationBase? parent)
	{
		Parent = parent;
	}

	public float CalculateFrom(float x, float y, int idx, int idy)
	{
		if (Parent == null)
		{
			return float.MaxValue;
		}
		else
		{
			return Parent.Calculate(x, y, idx, idy);
		}
	}

	public abstract float Calculate(float x, float y, int idx = 0, int idy = 0);

	protected static float Min(float a, float b)
	{
		//this can be smooth min, etc.
		return MathF.Min(a, b);
	}

	public abstract object Clone();
}