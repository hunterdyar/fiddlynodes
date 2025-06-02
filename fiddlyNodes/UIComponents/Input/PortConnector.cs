using System.Numerics;
using fiddlyNodes.NodeElements;
using Raylib_cs;

namespace fiddlyNodes;

/// <summary>
/// wire manager does the real stuff, this is just extending input handling.
/// A part of the input system, handles dragging, dropping, and calling the connect function on ports.
/// </summary>
public class PortConnector
{
	//are we wiggling a wire around?
	public bool Connecting => _connecting;
	private bool _connecting;

	public Port? FromPort => _fromPort;
	private Port? _fromPort;
	public Port? ToPort => _toPort;
	private Port? _toPort;
	
	private readonly InputManager _inputManager;
	private GridCanvas _gridCanvas;
	public PortConnector(InputManager inputManager)
	{
		_inputManager = inputManager;
	}

	public void StartDrag(Port _port)
	{
		_connecting = true;
		if (_port.IsOutput)
		{
			_fromPort = _port;
		}else if (_port.IsInput)
		{
			_toPort = _port;
		}
	}

	public void Draw()
	{
		if (!_connecting)
		{
			return;	
		}

		Vector2 from = _fromPort == null ? _inputManager.MousePosition : _fromPort.Transform.WorldPivotPosition;
		Vector2 to = _toPort == null ? _inputManager.MousePosition : _toPort.Transform.WorldPivotPosition;
		
		UIDraw.UIDrawHelpers.DrawWire(from,to, 2, Color.Black);
	}

	public void StopDrag(Port? _port)
	{
		if (!_connecting)
		{
			return;
			
		}
		if (_port == null)
		{
			_connecting = false;
			_fromPort = null;
			_toPort = null;
			return;
		}
		//dragging from or dragging to?
		
		//from to from, invalid.
		if (_port.IsInput && _fromPort == null)
		{
			return;
		}

		//to to to, invalid.
		if (_port.IsOutput && _toPort == null)
		{
			return;
		}

		bool valid = false;
		if (_fromPort != null)
		{
			if (_fromPort.CanConnectTo(_port))
			{
				var command = new ConnectPortsCommand(_fromPort, _port);
				Program.Commands.AddAndExecute(command);
			}
		}
		else if(_toPort != null)
		{
			if (_port.CanConnectTo(_toPort))
			{
				var command = new ConnectPortsCommand(_port, _toPort);
				Program.Commands.AddAndExecute(command);
			}
		}

		_fromPort = null;
		_toPort = null;
		_connecting = false;
	}
}