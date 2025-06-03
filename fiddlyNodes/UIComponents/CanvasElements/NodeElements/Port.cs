using System.Collections;
using System.Numerics;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class Port : Element
{
	private PortPosition _portPosition;
	private NodeProperty _nodeProperty;
	private float portSize = 2;
	public bool IsInput => _portPosition == PortPosition.Input;
	public bool IsOutput => _portPosition == PortPosition.Output;

	private WireManager wireManager => _nodeProperty.Node.Grid.WireManager;
	public NodeProperty Property => _nodeProperty;

	public Port(NodeProperty nodeProp, PortPosition portPosition) : base(0, 0, 4, 4)
	{
		//being a child of another element should be enough?
		_nodeProperty = nodeProp;
		_portPosition = portPosition;
		_transform.Pivot = new Vector2(0.5f, 0.5f);
	}
	
	public void Recalculate()
	{
		float d = _nodeProperty.Transform.Size.Y / 1.5f;

		if (IsInput)
		{
			//this happens after base() right? :p
			_transform.LocalPosition = new Vector2(0, _nodeProperty.Transform.Size.Y / 2);
			_transform.Size = new Vector2(d, d);
		}
		else if (IsOutput)
		{
			_transform.LocalPosition = new Vector2(_nodeProperty.Transform.Size.X, _nodeProperty.Transform.Size.Y / 2);
			_transform.Size = new Vector2(d, d);
		}
	}

	public override void Draw()
	{
		var world = _transform.WorldPivotPosition;
		float r = _transform.Size.X / 2;
		Raylib.DrawCircle((int)world.X,(int)world.Y,r, _hovering ? Color.DarkPurple : Color.Blue);
		base.Draw();
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		if (inputEvent is { Type: InputEventType.MouseLeftDown, Position: not null })
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				//check children. if the input isn't handled by them, then we can drag.
				inputEvent.Manager.RequestFocus(this);
				
				//startDrag....
				inputEvent.Manager.PortConnector.StartDrag(this);
				
				inputEvent.Handle();
			}
		}else if (inputEvent is { Type: InputEventType.MouseLeftUp, Position: not null })
		{
			inputEvent.Manager.PortConnector.StopDrag(this);
		}else if (inputEvent is { Type: InputEventType.MouseRightDown, Position: not null })
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				wireManager.ClearWires(this);
				inputEvent.Handle();
			}
		}

		//I can be hovered on
		else if (inputEvent is { Type: InputEventType.Hover })
		{
			inputEvent.Manager.RequestHover(this);
			inputEvent.Handle();
		}
	}

	public bool CanConnectTo(Port other)
	{
		if (!other._nodeProperty.CanConnectTo(_nodeProperty))
		{
			return false;
		}
		
		return other.IsInput != IsInput;
	}

	/// <summary>
	/// Connects port with a wire (creates and executes command)
	/// </summary>
	/// <param name="other"></param>
	/// <exception cref="Exception"></exception>
	public void ConnectTo(Port other)
	{
		var removeWires = new List<Wire>();
		wireManager.GetWiresThatCantCoexist(this, ref removeWires);
		wireManager.GetWiresThatCantCoexist(other, ref removeWires);
		if (IsInput)
		{
			throw new Exception("Connection should be called on output ports (from), not inputs.");
		}

		Program.Commands.AddAndExecute(new ConnectPortsCommand(this, other, removeWires));
	}
	
	/// <summary>
	/// Connects port with a wire. (not part of command system, should be called by command system)
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public Wire DoConnectTo(Port other)
	{
		var wire = new Wire(this, other, wireManager);
		wireManager.DoAddWire(wire);		
		return wire;
	}

	public IEnumerable<NodeProperty> PropertiesFrom()
	{
		return wireManager.PropertyFromEnumerable(this);
	}

	public bool IsConnected()
	{
		return wireManager.HasIncomingConnection(this);
	}
}