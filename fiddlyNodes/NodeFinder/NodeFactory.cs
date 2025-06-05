using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text;

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
	//https://stackoverflow.com/questions/1458468/youtube-like-guid
	
	public static string GetNodeDisplayNameFromType(Type nodeType)
	{
		var displayNameP = nodeType.GetProperty("DisplayName",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
		if (displayNameP == null)
		{
			return String.Empty;
		}

		var dnm = displayNameP?.GetValue(null, null);

		if (dnm is not string displayName)
		{
			Debug.WriteLine($"NodeFinder: Node {nodeType.FullName} has no displayName, skipping.");
			return string.Empty;
		}
		return displayName;
	}

	public static string[] GetNodeAliasesFromType(Type nodeType)
	{
		var aliasP = nodeType.GetProperty("Aliases",
			BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public);
		var aliases = aliasP.GetMethod.Invoke(null, null) as string[];
		if (aliases == null)
		{
			return [];
		}
		return aliases;
	}

	public static string SerializeNode(Node node)
	{
		// StringBuilder sb = new StringBuilder();
		// sb.Append("{");
		// sb.AppendLine("node: ");
		//{
		//node
		//type
		//properties = [
		
		//]
		//}{
		//wires[
		//from nodeid/name
		//to nodeid/name
		//
		return node.UID;
	}
	
	public static string GetUniqueID(){
			Thread.Sleep(1);
			string characterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			var charSet = characterSet.ToCharArray();
			int targetBase = charSet.Length;
			long ticks = (long)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			string output = null;

			do
			{
				output += charSet[ticks % targetBase];
				ticks = ticks / targetBase;
			} while (ticks > 0);

			output = new string(output.Reverse().ToArray());

			return Convert.ToBase64String(Encoding.UTF8.GetBytes(output)).Replace("/", "_")
				.Replace("+", "-").Replace("==", "");
	}
	
}
