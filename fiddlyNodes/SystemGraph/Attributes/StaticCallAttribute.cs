using fiddlyNodes.Thistle;

namespace Thistle.Library;


public class StaticCallAttribute : Attribute
{
	//i'm not using attributes right now and the assembly queries are not cached and they are slow.
	public ThistleType CallOnType;
	public ThistleType ReturnType;
	public ThistleType[] ParameterTypes;

	public StaticCallAttribute(ThistleType callOnType, ThistleType returnType, params ThistleType[] parameterTypes)
	{
		CallOnType = callOnType;
		ReturnType = returnType;
		ParameterTypes = parameterTypes;
	}
}