using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json;

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

	public static NodeData? NodeToDataObject(Node node)
	{
		var aliases = GetNodeAliasesFromType(node.GetType());
		if (aliases.Length == 0)
		{
			return null;
		}
		var nodeType = aliases[0];

		//todo: need to create a path system for each node. give it an optional id in constructor, and it passes to it's children.
		var properties = node.GetPropertiesForSerialization().Distinct();
		var nodeData = new NodeData()
		{
			ID = node.UID,
			Type = nodeType,
			x = node.Transform.LocalPosition.X,
			y = node.Transform.LocalPosition.Y,
			//node.properties needs to get subProperties. GetPath needs have all of them,
			
			//we only need to serialize properties that have been set Connected ones can/should be ignored.
			Properties = properties.Select(x => x.GetPath(false)).ToArray(),
			Values = properties.Select(x => x.ToString()).ToArray(),
			
		};
		return nodeData;
	}

	public static string SerializeProgram()
	{
		var pd = GetProgramData();
		return System.Text.Json.JsonSerializer.Serialize(pd, new JsonSerializerOptions()
		{
			WriteIndented = true
		});
	}

	public static void DeserializeProgram(Stream saveData, bool clearFirst)
	{
		if (clearFirst)
		{
			foreach (var wire in Program.GridCanvas.WireManager.GetAllWires())
			{
				Program.GridCanvas.WireManager.RemoveWire(wire);
			}

			foreach (var node in Program.GridCanvas.GetAllNodes().ToArray())
			{
				Program.GridCanvas.RemoveNode(node);
			}
		}
		
		var pd = System.Text.Json.JsonSerializer.Deserialize<ProgramData>(saveData);
		foreach (var nodeData in pd.NodeData)
		{
			nodeData.CreateNode();
		}

		foreach (WireData wireData in pd.WireData)
		{
			Program.GridCanvas.WireManager.CreateWireFromData(wireData);
		}
	}
	public static ProgramData GetProgramData()
	{
		var allNodes = Program.GridCanvas.GetAllNodes();
		var p = new ProgramData();
		p.NodeData = new List<NodeData>();
		foreach (var node in allNodes)
		{
			var nd = NodeToDataObject(node);
			if (nd != null)
			{
				p.NodeData.Add(nd.Value);
			}
		}

		var allWires = Program.GridCanvas.WireManager.GetAllWires();
		p.WireData = new List<WireData>();
		foreach (var wire in allWires)
		{
			var wd = WireToDataObject(wire);
			if (wd != null)
			{
				p.WireData.Add(wd.Value);
			}
		}

		return p;
	}
	public static WireData? WireToDataObject(Wire wire)
	{
		if (wire.FromPort == null || wire.ToPort == null)
		{
			return null;
		}
		
		return new WireData()
		{
			FromPath = wire.FromPort.GetPathID(),
			ToPath = wire.ToPort.GetPathID(),
		};
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
	public struct ProgramData
	{
		public List<NodeData> NodeData { get; set; }
		public List<WireData> WireData { get; set; }
	}
	
	[Serializable]
	public struct NodeData
	{
		public string ID { get; set; }
		public string Type { get; set; }
		
		public float x { get; set; }
		public float y { get; set; }

		public string[] Properties {get;set;}
		public string[] Values {get;set;}

		public void CreateNode()
		{
			if (Program.NodeFinder.nodeItems.TryGetValue(Type, out var node))
			{
				var newNode = node.CreateNodeFunction.Invoke();
				newNode.UID = ID;
				newNode.Transform.LocalPosition = new Vector2(x,y);
			}
		}
	}

	[Serializable]
	public struct WireData
	{
		public string FromPath { get; set; }
		public string ToPath { get; set; }
	}
}
