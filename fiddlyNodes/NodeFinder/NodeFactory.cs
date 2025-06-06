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
		var aliases = GetNodeAliasesFromType(node.GetType());
		if (aliases.Length == 0)
		{
			return String.Empty;
		}
		var nodeType = aliases[0];

		//todo: need to create a path system for each node. give it an optional id in constructor, and it passes to it's children.
		var properties = node.GetPropertiesForSerialization();
		var nodeData = new NodeData()
		{
			ID = node.UID,
			Type = nodeType,
			//node.properties needs to get subProperties. GetPath needs have all of them,
			
			//we only need to serialize properties that have been set Connected ones can/should be ignored.
			Properties = properties.Select(x => x.GetPath()).ToArray(),
			Values = properties.Select(x => x.ToString()).ToArray()
		};
		return System.Text.Json.JsonSerializer.Serialize(nodeData);
		
		return node.UID;
	}

	public static void DeserializeData(string saveData)
	{
		//get the section on nodes.
		//get the section on wires.
		//get the section on options.
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

	[Serializable]
	public struct NodeData
	{
		public string ID { get; set; }
		public string Type { get; set; }
		public string[] Properties {get;set;}
		public string[] Values {get;set;}
	}
}
