using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes;

public class UnitField : OptionsField<TUnit>
{
	public UnitField(int x, int y, int width, int height) : base(x, y, width, height)
	{
		Options = new List<TUnit>(TUnit.AllUnits);
		_selectedIndex = 0;
		Selected = Options[_selectedIndex];
	}

	public override string ToString()
	{
		return Selected.ToString();
	}

	public void SetFromString(String value)
	{
		for (var i = 0; i < Options.Count; i++)
		{
			TUnit unit = Options[i];
			if (unit.ToString() == value)
			{
				_selectedIndex = i;
				Selected = Options[_selectedIndex];
				return;
			}
		}
		throw new Exception("unable to deserialize units field.");
	}
}