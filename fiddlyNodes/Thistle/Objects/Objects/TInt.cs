using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;
public class TInt : TreeNativeObject<int>
{
	public TInt(int value) : base(value)
	{
	}

	public override ThistleType TType => ThistleType.tint;

	public override void SetValue(TreeBaseObject value)
	{
		if (value is TInt ti)
		{
			Value = ti.Value;
			return;
		}

		if (value is TFloat tf)
		{
			// Value = floattf.Value;
		}

		throw new InvalidCastException($"Unable to convert {value} ({value.TType}) to int");

	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public override TreeBaseObject Clone()
	{
		return new TInt(Value);
	}
}