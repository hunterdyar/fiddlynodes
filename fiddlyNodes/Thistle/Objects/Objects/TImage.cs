using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.Thistle;
public class TImage : TreeNativeObject<Raylib_cs.Image>
{
	public TImage(Raylib_cs.Image image) : base(image)
	{
	}

	public override string ToString()
	{
		return $"Image ({Value.Width}x{Value.Height})";
	}

	public override TreeBaseObject Clone()
	{
		return new TImage(Value);
	}
}