using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class TextField : Element, IChangeReporter<TString>
{
	public string TextValue => _text.Value;
	protected TextInputHandler _text;
	public Action<TString> OnChange { get; set; }

	
	public TextField(int x, int y, int width, int height) : base(x, y, width, height)
	{
		_text = new TextInputHandler(IsValidInputCharacter);
	}

	public override void Draw()
	{
		var bounds = _transform.WorldBounds;
		//todo: draw-text-in-bounds utility.
		int fontSize = (int)(bounds.Height * 0.9f);
		float letterWidth = fontSize/2;
		if (_text.Value.Length > 0)
		{
			letterWidth = float.Ceiling(Raylib.MeasureText(_text.Value, fontSize) / (float)_text.Value.Length);
		}
		int vpad = (int)Math.Floor(bounds.Height - fontSize) / 2; //vertically center
		
		Raylib.DrawRectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, Color.RayWhite);
		Raylib.DrawRectangleLines((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, _focused ? Color.Gray : Color.Black);
		Raylib.DrawText(_text.Value, (int)bounds.X, (int)bounds.Y+vpad, (int)fontSize,Color.Black);
		if (_focused)
		{
			var cursorPosition = (int)(_text.CursorPos*letterWidth);
			Raylib.DrawLine((int)bounds.X+cursorPosition+2, (int)bounds.Y+1, (int)bounds.X + cursorPosition+1, (int)bounds.Y+(int)bounds.Height-3, Color.Gray);
			Raylib.DrawLine((int)bounds.X + cursorPosition+2, (int)bounds.Y + (int)bounds.Height - 3, (int)(bounds.X + cursorPosition+letterWidth+2), (int)bounds.Y + (int)bounds.Height - 3, Color.Gray);

		}
		base.Draw();
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		if (_focused && inputEvent.Type == InputEventType.KeyPress)
		{
			_text.OnInput(ref inputEvent);

		}else if (inputEvent.Type == InputEventType.MouseLeftDown && inputEvent.Position != null)
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				inputEvent.Manager.RequestFocus(this);
				inputEvent.Handle();
			}
		}
		base.OnInput(ref inputEvent);
	}

	protected virtual bool IsValidInputCharacter(char c)
	{
		return char.IsAsciiLetterOrDigit(c) || c == '.' || c == ' ' || c == '-' || c == '_';
	}

	public void SetValue(string s, bool immediate = false)
	{
		_text.SetValue(s,immediate);
	}
}