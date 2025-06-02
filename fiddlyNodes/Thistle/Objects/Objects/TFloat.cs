using System.Globalization;

using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;
public class TFloat : TreeNativeObject<float>
{
	public TFloat(float value) : base(value)
	{
	}

	public override ThistleType TType => ThistleType.Tfloat;

	public override void SetValue(TreeBaseObject value)
	{
		if (value is TFloat floatValue)
		{
			Value = floatValue.Value;
			return;
		}

		if (value is TInt intValue)
		{
			Value = intValue.Value;
		}

		if (value is TString stringValue)
		{
			throw new InvalidCastException($"Can't cast object from {value} to Float");
		}
		
		
		base.SetValue(value);
	}

	public override string ToString()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}

	public override TreeBaseObject Clone()
	{
		return new TFloat(Value);
	}
}