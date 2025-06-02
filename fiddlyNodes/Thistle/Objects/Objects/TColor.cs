using System.Drawing;
using Color = Raylib_cs.Color;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;
public class TColor : TreeNativeObject<Color>
{
	public TColor(Color value) : base(value)
	{
	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public override TreeBaseObject Clone()
	{
		return new TColor(Value);
	}
}