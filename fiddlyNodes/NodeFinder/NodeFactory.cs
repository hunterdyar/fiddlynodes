using System.Numerics;

namespace fiddlyNodes;

public static class NodeFactory
{
	public static Node CreateNode(Type type, Vector2 mp, GridCanvas gridCanvas)
	{
		var constructor = type.GetConstructor([typeof(int), typeof(int), typeof(GridCanvas)]);
		if (constructor != null)
		{
			return constructor.Invoke([(int)mp.X, (int)mp.Y, gridCanvas]) as Node;
		}
		else
		{
			Console.WriteLine($"Can't create constructor for {type.GetMember("DisplayName")[0].Name}");
			return null;
		}
	}

}