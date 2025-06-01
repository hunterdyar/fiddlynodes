using fiddlyNodes.Thistle;

namespace fiddlyNodes.Thistle;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
public class BuiltInAttribute : Attribute
{
	public ThistleType Type;
	public ThistleType[] Params = new ThistleType[] { };

	public BuiltInAttribute(ThistleType type)
	{
		this.Type = type;
	}

	public BuiltInAttribute(ThistleType type, params ThistleType[] ps)
	{
		this.Type = type;
		this.Params = ps;
	}
}