using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;
public class TBool : TreeNativeObject<bool>
{
	public override void SetValue(TreeBaseObject other)
	{
		if (other is TBool tb)
		{
			Value = tb.Value;
			return;
		}

		if (other is TInt ti)
		{
			Value = ti.Value > 0;//todo: this sure is a decision...
			return;
		}
		throw new InvalidOperationException($"Can't set bool value of {this} to {other}. Invalid Cast.");
	}

	public override TreeBaseObject Clone()
	{
		return new TBool(Value);
	}

	public TBool(bool value) : base(value)
	{
	}

	public override string ToString()
	{
		//prefer lowercase
		return Value ? "true" : "false";
	}
}