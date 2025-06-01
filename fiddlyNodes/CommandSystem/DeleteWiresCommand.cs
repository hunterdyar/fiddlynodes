using fiddlyNodes.NodeElements;

namespace fiddlyNodes;

public class DeleteWiresCommand : Command
{
	private List<Wire> _wires;
	private readonly List<(Port, Port)> _disconnected = new List<(Port, Port)>();
	
	
	public DeleteWiresCommand(Wire wire)
	{
		_wires = new List<Wire>(){wire};
	}

	public DeleteWiresCommand(HashSet<Wire> wires)
	{
		_wires = new List<Wire>(wires);
	}

	public DeleteWiresCommand(List<Wire> wires)
	{
		_wires = new List<Wire>(wires);
	}

	public override void Execute()
	{
		base.Execute();
		_disconnected.Clear();
		for (int i = 0; i < _wires.Count; i++)
		{
			_disconnected.Add((_wires[i].FromPort, _wires[i].ToPort));
			_wires[i].Remove();
		}
		_wires.Clear();
	}

	public override void Undo()
	{
		base.Undo();
		foreach (var ptp in _disconnected)
		{
			var w = ptp.Item1.DoConnectTo(ptp.Item2);
			_wires.Add(w);
		}
		_disconnected.Clear();
		_state = CommandState.Undone;
	}
}