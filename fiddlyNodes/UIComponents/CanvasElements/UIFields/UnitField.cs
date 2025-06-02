using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes;

public class UnitField : OptionsField<TUnit>
{
	public UnitField(int x, int y, int width, int height) : base(x, y, width, height)
	{
		Options = new List<TUnit>(TUnit.AllUnits);
	}
}