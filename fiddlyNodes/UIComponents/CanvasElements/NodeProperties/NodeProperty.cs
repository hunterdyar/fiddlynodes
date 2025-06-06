using System.Numerics;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

//A nodeProperty is a property that takes up the full width of the node. It might have an input port or an output port. or both, hypothetically.
public abstract class NodeProperty : Element
{
	protected Port? InputPort;
	protected Port? OutputPort;
	public int MinWidth;
	public string PropertyName => propertyName;
	protected string propertyName;
	public Node Node => _node;
	private Node _node;

	protected NodeProperty _parentProperty;
	
	//number of prop-heights the property takes.
	public float PropHeight = 1;
	public NodeProperty(string propertyName, Node node, NodeProperty? parent = null) : base(0, 0, 20, 12)
	{
		_node = node;
		///temp testing data that will be in child classes.
		this.propertyName = propertyName;
		MinWidth = propertyName.Length * Raylib.GetFontDefault().BaseSize;
		_transform.ScaleWithParent = true;
		_parentProperty = parent;
	} 

	public virtual void Recalculate()
	{
		InputPort?.Recalculate();
		OutputPort?.Recalculate();
	}
	
	public override void Draw()
	{
		var w = _transform.WorldBounds;
		int hpad = 5; //get port size/2+pad.

		if (InputPort != null)
		{
			//.5 for port radius + .1 extra
			hpad = (int)(InputPort.Transform.Size.X * 0.6);
		}
		//debug:
		// Raylib.DrawRectangleLines((int)w.X,(int)w.Y,(int)w.Width,(int)w.Height,Color.DarkPurple);
		
		if (InputPort != null)
		{
			InputPort.Draw();
		}
		if (OutputPort != null)
		{
			OutputPort.Draw();
		}
	}
	
	public void AddAndSetPort(Port port)
	{
		AddChild(port);
		if (port.IsInput)
		{
			InputPort = port;
		}
		else if (port.IsOutput)
		{
			OutputPort = port;
		}
		else
		{
			throw new Exception("Can't add a none port to property.");
		}
	}
	
	public override void OnInput(ref InputEvent inputEvent)
	{
		base.OnInput(ref inputEvent);

		if (inputEvent is { Type: InputEventType.MouseLeftUp, Position: not null })
		{
			if (InputPort != null)
			{
				inputEvent.Manager.PortConnector.StopDrag(InputPort);
			}else if (OutputPort != null)
			{
				inputEvent.Manager.PortConnector.StopDrag(OutputPort);
			}
		}
	}

	public virtual bool CanConnectTo(NodeProperty nodeProperty)
	{
		return true;
	}

	public void ClearAllConnections()
	{
		//todo: return commands for the command system.
		if (InputPort != null)
		{
			_node.Grid.WireManager.ClearWires(InputPort);
		}else if (OutputPort != null)
		{
			_node.Grid.WireManager.ClearWires(OutputPort);
		}
	}

	public string GetPath(bool includeNode)
	{
		if (_parentProperty == null)
		{
			return (includeNode ? _node.UID+"/" : "/") + propertyName;
		}
		else
		{
			return _parentProperty.GetPath(includeNode)+"/"+propertyName;
		}
	}

	public virtual IEnumerable<NodeProperty> GetProperties()
	{
		yield return this;
	}

	public virtual void SetValueFromString(string value)
	{
		throw new NotImplementedException();
	}
}

public abstract class NodeProperty<T> : NodeProperty where T : TreeBaseObject
{
	protected NodeProperty(string propertyName, Node node) : base(propertyName, node)
	{
	}

	public override bool CanConnectTo(NodeProperty nodeProperty)
	{
		if (nodeProperty is NodeProperty<T> prop)
		{
			return base.CanConnectTo(prop);
		}

		return false;
	}

	public virtual T GetValue()
	{
		throw new NotImplementedException();
	}
}