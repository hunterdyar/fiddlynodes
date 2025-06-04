using System.Collections;
using System.Numerics;
using fiddlyNodes.NodeElements;

namespace fiddlyNodes;

public class WireManager
{
	private Dictionary<Port, HashSet<Wire>> _outgoingWires = new Dictionary<Port, HashSet<Wire>>();
	private Dictionary<Port, HashSet<Wire>> _incomingWires = new Dictionary<Port, HashSet<Wire>>();
	private List<Wire> _markedForDelete = new List<Wire>();
	public WireManager()
	{
	}

	public void Draw(RectTransform transform)
	{
		foreach (var wires in _outgoingWires.Values)
		{
			foreach (var wire in wires)
			{
				wire.Draw();
			}
		}
	}

	public void RemoveWire(Wire wire)
	{
		//remove from both our lists.
		var f = wire.FromPort;
		if (_outgoingWires.ContainsKey(f))
		{
			_outgoingWires[f].Remove(wire);
		}
		else
		{
			throw new Exception("Can't find wire to remove.");
		}

		f = wire.ToPort;
		if (_incomingWires.ContainsKey(f))
		{
			_incomingWires[f].Remove(wire);
		}
		else
		{
			throw new Exception("Can't find wire to remove.");
		}

	}

	public void DoAddWire(Wire wire)
	{
		if (_outgoingWires.ContainsKey(wire.FromPort))
		{
			var list = _outgoingWires[wire.FromPort];
			_outgoingWires[wire.FromPort].Add(wire);
		}
		else
		{
			_outgoingWires.Add(wire.FromPort, new HashSet<Wire>() { wire });
		}

		if (_incomingWires.ContainsKey(wire.ToPort))
		{
			_incomingWires[wire.ToPort].Add(wire);
		}
		else
		{
			_incomingWires.Add(wire.ToPort, new HashSet<Wire>() { wire });
		} 
	}

	/// <summary>
	/// OnInput but called by peers in the hierarchy, not by InputManager directly.
	/// </summary>
	public void PassInput(ref InputEvent inputEvent)
	{
		if (inputEvent.Type == InputEventType.Hover)
		{
			if (GetWireAtPoint(inputEvent.Position.Value, out var wire))
			{
				inputEvent.Manager.RequestHover(wire);
				inputEvent.Handle();
			}
		}else if (inputEvent.Type == InputEventType.MouseRightDown)
		{
			if (inputEvent.Manager.Hovering is Wire wire)
			{
				if (!_markedForDelete.Contains(wire))
				{
					wire.SetMarkedForDelete(true);
					_markedForDelete.Add(wire);
					inputEvent.Handle();
				}
			}
		}else if (inputEvent.Type == InputEventType.MouseRightUp)
		{
			foreach (var wire in _markedForDelete)
			{
				wire.SetMarkedForDelete(false);
			}
			var c = new DeleteWiresCommand(_markedForDelete);
			Program.Commands.AddAndExecute(c);
			_markedForDelete.Clear();
		}
		else if (inputEvent.Type == InputEventType.MouseMove && inputEvent.Manager.IsRightMouseDown)
		{
			if (inputEvent.Manager.Hovering is Wire wire)
			{
				wire.SetMarkedForDelete(true);
				_markedForDelete.Add(wire);
				inputEvent.Handle();
			}
		}
	}

	public bool GetWireAtPoint(Vector2 pos, out Wire wire)
	{
		foreach (var wires in _outgoingWires.Values)
		{
			foreach (Wire w in wires)
			{
				if (w.OverlapsPoint(pos))
				{
					wire = w;
					return true;
				}
			}
		}
		
		wire = null;
		return false;
	}

	public void ClearWires(Port port)
	{
		if (port.IsInput)
		{
			if(_incomingWires.TryGetValue(port, out var w))
			{
				var d = new DeleteWiresCommand(w);
				Program.Commands.AddAndExecute(d);
			}
		}else if(port.IsOutput)
		{
			if (_outgoingWires.TryGetValue(port, out var w))
			{
				var d = new DeleteWiresCommand(w);
				Program.Commands.AddAndExecute(d);
			}
		}
	}

	public void GetWiresThatCantCoexist(Port other, ref List<Wire> wires)
	{
		//ports can only have one input.
		if (other.IsOutput)
		{
			//if only one output and wire exits, clear. we don't have that port setting yet.
			return;
		}

		if (!other.IsInput)
		{
			return;
		} //check for invalid ports.

		//right now, only one input is allowed.
		if (_incomingWires.TryGetValue(other, out var w))
		{
			foreach (var wire in w)
			{
				wires.Add(wire);
			}
		}

		return;
	}

	public IEnumerable<NodeProperty> PropertyFromEnumerable(Port port)
	{
		if (_incomingWires.TryGetValue(port, out var w))
		{
			foreach (var wire in w)
			{
				yield return wire.FromPort.Property;
			}	
		}
		else
		{
			yield break;
		}
	}

	public bool HasIncomingConnection(Port port)
	{
		if (_incomingWires.TryGetValue(port, out var value))
		{
			return value.Count != 0;
		}

		return false;
	}

	public List<Wire> GetAllWires()
	{
		return (from sets in _outgoingWires.Values
			from item in sets
			select item).ToList();
	}
}