namespace fiddlyNodes.Thistle.Library;

public class TString : TreeNativeObject<string>
{
	public TString(string value) : base(value)
	{
	}

	public override ThistleType TType => ThistleType.tstring;

	public override void SetValue(TreeBaseObject value)
	{
		if (value is TString ts)
		{
			this.Value = ts.Value;
			return;
		}

		if (value is TInt ti)
		{
			this.Value = ti.Value.ToString();
			return;
		}

		if (value is TFloat tf)
		{
			this.Value = tf.Value.ToString();
			return;
		}
		throw new InvalidCastException($"Cannot cast {value} to string");
	}

	public override string ToString()
	{
		return Value;
	}

	public override TreeBaseObject Clone()
	{
		return new TString(Value);
	}
}
	