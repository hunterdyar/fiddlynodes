using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle.Library;

public class TUnit : TreeNativeObject<Unit>
{
	public static readonly TUnit[] AllUnits =
	[
		new TUnit(Unit.Pixels), 
		new TUnit(Unit.PercentWidth),
		new TUnit(Unit.PercentHeight)
	];
	
	public TUnit(Unit value) : base(value)
	{
	}
	

	public override TreeBaseObject Clone()
	{
		return new TUnit(Value);
	}

	public override string ToString()
	{
		switch(Value)
		{
			case Unit.Pixels: return "Pixels";
			case Unit.PercentWidth: return "%w";
			case Unit.PercentHeight: return "%h";
		}
		return base.ToString();
	}
}