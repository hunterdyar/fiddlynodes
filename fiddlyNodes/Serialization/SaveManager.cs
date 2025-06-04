using System.Text.Json;

namespace fiddlyNodes;

public class SaveManager
{
	[Serializable]
	public class SaveFile
	{
		public string date { get; set; }
		public List<Node> Nodes { get; set; }
		public List<Wire> Wires { get; set; }
	}
	public void Save()
	{
		var nodes = Program.GridCanvas.GetAllNodes();
		var wires = Program.GridCanvas.WireManager.GetAllWires();

		SaveFile saveFile = new SaveFile()
		{
			date = DateTime.Now.ToString("yyyy-MM-dd"),
			Nodes = nodes,
			Wires = wires
		};
		
		//todo write our own ToJson and FromJson handlers for nodes, we will need some ID or path system for nodes/props to do the wires more cleanly.
		
		// var output = JsonSerializer.Serialize<SaveFile>(saveFile);
		// Console.WriteLine();
	}
}