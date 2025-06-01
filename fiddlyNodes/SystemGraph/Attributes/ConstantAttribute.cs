using fiddlyNodes.Thistle;

namespace fiddlyNodes.Thistle;

public class ConstantAttribute : Attribute
{
	private ThistleType Type;

	public ConstantAttribute(ThistleType type)
	{
		Type = type;
	}
}