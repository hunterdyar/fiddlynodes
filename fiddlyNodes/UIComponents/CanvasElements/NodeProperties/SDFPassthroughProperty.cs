using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes.NodeElements;

public class SDFPassthroughProperty : NodeProperty<TSDF>
{
	private SDFOperationBase[] _operations;
	public SDFPassthroughProperty(string propertyName, Node node) : base(propertyName, node)
	{
		AddAndSetPort(new Port(this, PortPosition.Input));
		AddAndSetPort(new Port(this, PortPosition.Output));
	}

	public override TSDF GetValue()
	{
		TSDF incoming = null;
		if (InputPort != null && InputPort.IsConnected())
		{
			foreach (var nodeProperty in InputPort.PropertiesFrom())
			{
				if (nodeProperty is NodeProperty<TSDF> sdfProp)
				{
					incoming = sdfProp.GetValue();
					//return the first value, since only one connection should exist.
					break;
				}
				else
				{
					throw new InvalidCastException("Invalid node connection");
				}
			}
		}

		if (incoming == null)
		{
			incoming = new TSDF();
		}
		
		if (_operations != null)
		{
			foreach (SDFOperationBase operation in _operations)
			{
				incoming.AddOperation(operation);
			}
		}
		
		return incoming;
	}

	public void AddOp(SDFOperationBase operation)
	{
		if (_operations == null)
		{
			_operations = [operation];
		}
		else
		{
			//extend the array. we don't expect this to get resized very often, and almost all passthroughs will be just one operation.
			
			var ops = new SDFOperationBase[_operations.Length + 1];
			Array.Copy(_operations, 0, ops, 0, _operations.Length);
			ops[-1] = operation;
			_operations = ops;
		}
	}
}