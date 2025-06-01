using System.Numerics;
using Raylib_cs;

namespace fiddlyNodes;

public class InputEvent
{
	public InputManager Manager;
	public InputEventType Type;
	public Vector2? Position = null;
	public Vector2? Delta = null;
	public KeyboardKey? KeyboardKey = null;
	public char? Character = null;
	public bool Handled { get; private set;}

	public InputEvent(InputManager manager)
	{
		Handled = false;
		Manager = manager;
	}
	public void Handle()
	{
		Handled = true;
	}
}