using System.Numerics;
using Raylib_cs;

namespace fiddlyNodes;

public class InputManager
{
	private Element? _hasFocus;
	public IHoverable? Hovering => _hasHover;
	private IHoverable? _hasHover;
	private Element? _dragging;
	
	private List<Element> _hoverierarchy = new List<Element>();
	public bool IsLeftMouseDown;
	private bool prevLeftMouseDown;
	public bool IsRightMouseDown;
	private bool prevRightMouseDown;
	public bool IsMiddleMouseDown;
	private bool prevMiddleMouseDown;

	public Vector2 MousePosition;
	public Vector2 MouseDelta;

	public PortConnector PortConnector => _portConnector;

	private PortConnector _portConnector;

	public InputManager()
	{
		_portConnector = new PortConnector(this);
	}
	public void Tick()
	{
		IsLeftMouseDown = Raylib.IsMouseButtonDown(MouseButton.Left);
		IsRightMouseDown = Raylib.IsMouseButtonDown(MouseButton.Right);
		IsMiddleMouseDown = Raylib.IsMouseButtonDown(MouseButton.Middle);
		//update our references
		_hoverierarchy.Clear();
		Program.Root.GetChildElementOrSelfAtPosition(Raylib.GetMousePosition(), ref _hoverierarchy);
		_hoverierarchy.Reverse();
		
	
		MousePosition= Raylib.GetMousePosition();
		MouseDelta = Raylib.GetMouseDelta();
		
		if (Raylib.GetMouseWheelMoveV().Y != 0)
		{
			//create a mouse scroll event
			//fire the event on the input handler that is under the mouse.
			var scrollEvent = new InputEvent(this)
			{
				Type = InputEventType.MouseScroll,
				Position = MousePosition,
				Delta = Raylib.GetMouseWheelMoveV(),
			};
			PropagateInputToFocusedUnderMouse(scrollEvent);
		}

		if (prevLeftMouseDown != IsLeftMouseDown)
		{
			InputEventType t = InputEventType.MouseLeftDown;
			if (!IsLeftMouseDown)
			{
				t = InputEventType.MouseLeftUp;
			}

			var pressedEvent = new InputEvent(this)
			{
				Type = t,
				Position = Raylib.GetMousePosition(),
			};
			PropagateInputToFocusedUnderMouse(pressedEvent);
			
			//if we just released
			if (!IsLeftMouseDown && !pressedEvent.Handled)
			{
				//this will be false if the port called the stopdrag on release.
				if (_portConnector.Connecting)
				{
					//cancel connection.
					_portConnector.StopDrag(null);
				}
			}
		}

		if (prevRightMouseDown != IsRightMouseDown)
		{
			InputEventType t = InputEventType.MouseRightDown;
			if (!IsRightMouseDown)
			{
				t = InputEventType.MouseRightUp;
			}

			var pressedEvent = new InputEvent(this)
			{
				Type = t,
				Position = Raylib.GetMousePosition(),
			};
			PropagateInputToFocusedUnderMouse(pressedEvent);
		}

		if (prevMiddleMouseDown != IsMiddleMouseDown)
		{
			InputEventType t = InputEventType.MouseMiddleDown;
			if (!IsMiddleMouseDown)
			{
				t = InputEventType.MouseMiddleUp;
			}

			var pressedEvent = new InputEvent(this)
			{
				Type = t,
				Position = Raylib.GetMousePosition(),
			};
			PropagateInputToFocusedUnderMouse(pressedEvent);
		}

		var c = Raylib.GetKeyPressed();
		while (c != 0)
		{
			InputEventType t = InputEventType.KeyPress;

			var pressedEvent = new InputEvent(this)
			{
				Type = t,
				Position = Raylib.GetMousePosition(),
				KeyboardKey = (KeyboardKey)c,
				Character = (char)c,
			};
			PropagateKeyboardInput(pressedEvent);
			
			//
			c = Raylib.GetKeyPressed();
		}


		//non-zero
		if (float.Abs(MouseDelta.X) > float.Epsilon || float.Abs(MouseDelta.Y) > float.Epsilon)
		{
			var pressedEvent = new InputEvent(this)
			{
				Type = InputEventType.MouseMove,
				Position = MousePosition,
				Delta = MouseDelta
			};
			PropagateInputToFocusedUnderMouse(pressedEvent);
		}
		
		//now we just do the hovers.
		PropagateHoverInput();
		
		prevLeftMouseDown = IsLeftMouseDown;
		prevRightMouseDown = IsRightMouseDown;
		prevMiddleMouseDown = IsMiddleMouseDown;
	}

	private void PropagateHoverInput()
	{
		var inputEvent = new InputEvent(this)
		{
			Type = InputEventType.Hover,
			Position = MousePosition,
			Delta = MouseDelta,
		};

		if (!inputEvent.Handled)
		{
			foreach (var child in _hoverierarchy)
			{
				child.OnInput(ref inputEvent);
				if (inputEvent.Handled)
				{
					return;
				}
			}
		}
	}

	private void PropagateKeyboardInput(InputEvent inputEvent)
	{
		if (!inputEvent.Handled)
		{
			if (_hasFocus != null)
			{
				_hasFocus.OnInput(ref inputEvent);
			}

			if (inputEvent.Handled)
			{
				return;
			}
			
			Program.Commands.OnInput(inputEvent);
			if (inputEvent.Handled)
			{
				return;
			}
		}
	}

	public void RequestHover(IHoverable hoverable)
	{
		if (_hasHover == hoverable)
		{
			return;
		}

		_hasHover?.OnLoseHover();
		_hasHover = hoverable;
		_hasHover?.OnGainHover();
	}

	public void ClearHover()
	{
		_hasHover?.OnLoseHover();
		_hasHover = null;
	}
	
	private void PropagateInputToFocusedUnderMouse(InputEvent inputEvent)
	{
		if (_hasFocus != null)
		{
			_hasFocus.OnInput(ref inputEvent);
		}

		if (!inputEvent.Handled)
		{
			foreach (var child in _hoverierarchy)
			{
				child.OnInput(ref inputEvent);
				if (inputEvent.Handled)
				{
					return;
				}
			}
		}
	}

	public void RequestFocus(Element element)
	{
		if (_hasFocus == element)
		{
			return;
		}

		//release focus for current element
		_hasFocus?.OnLoseFocus();
		//set focus for new element.
		_hasFocus = element;
		_hasFocus.OnGainFocus();
	}

	public void ClearFocus()
	{
		_hasFocus?.OnLoseFocus();
		_hasFocus = null;
	}

	public void Draw()
	{
		_portConnector.Draw();
	}

	public void DebugDraw()
	{
		//Debug things
		if (_hoverierarchy.Count > 0)
		{
			var last = _hoverierarchy[0];
			Raylib.DrawRectangleLinesEx(last.Transform.WorldBounds, 3, Color.Blue);
		}

		if (_hasFocus != null)
		{
			Raylib.DrawRectangleLinesEx(_hasFocus.Transform.WorldBounds, 2, Color.Red);
		}

		Raylib.DrawText($"drag: {_dragging}, focus: {_hasFocus}, hovering: {_hasHover}", 30, 3, 12, Color.Red);

	}

}