using fiddlyNodes.NodeElements;

namespace fiddlyNodes;

public class ConnectPortsCommand : Command
{
	private Port _a;
	private Port _b;
	private Wire _createdWire;
	//subcommand, not part of manager/undos.
	private DeleteWiresCommand? _deleteWires;
	public ConnectPortsCommand(Port a, Port b, List<Wire> removeWires)
	{
		_a = a;
		_b = b;
		if (removeWires.Count > 0)
		{
			_deleteWires = new DeleteWiresCommand(removeWires);
		}
		else
		{
			_deleteWires = null;
		}
	}

	public ConnectPortsCommand(Port a, Port b)
	{
		_a = a;
		_b = b;
		_deleteWires = null;
	}

	public override void Execute()
	{
		if (_state == CommandState.Executed)
		{
			throw new Exception("Command already executed");
		}
		
		_deleteWires?.Execute();
		_createdWire = _a.DoConnectTo(_b);
		_state = CommandState.Executed;
	}

	public override void Undo()
	{
		if (_state != CommandState.Executed)
		{
			throw new Exception("Command not executed");
		}
		_deleteWires?.Undo();
		_createdWire.Remove();
		_state = CommandState.Undone;
	}
}