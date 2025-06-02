using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class TextField : Element, IChangeReporter<TString>
{
	public string Value => _value;
	private string _value = string.Empty;
	private int _cursorPosition;
	public Action<TString> OnChange { get; set; }

	
	public TextField(int x, int y, int width, int height) : base(x, y, width, height)
	{
	}

	public override void Draw()
	{
		var bounds = _transform.WorldBounds;
		//todo: draw-text-in-bounds utility.
		int fontSize = (int)(bounds.Height * 0.9f);
		float letterWidth = 0;
		if (_value.Length > 0)
		{
			letterWidth = float.Ceiling(Raylib.MeasureText(_value, fontSize) / (float)_value.Length);
		}
		int vpad = (int)Math.Floor(bounds.Height - fontSize) / 2; //vertically center
		
		Raylib.DrawRectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, Color.RayWhite);
		Raylib.DrawRectangleLines((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, _focused ? Color.Gray : Color.Black);
		Raylib.DrawText(_value, (int)bounds.X, (int)bounds.Y+vpad, (int)fontSize,Color.Black);
		if (_focused)
		{
			var cursorPosition = (int)(_cursorPosition*letterWidth);
			Raylib.DrawLine((int)bounds.X+cursorPosition, (int)bounds.Y+1, (int)bounds.X + cursorPosition, (int)bounds.Y+(int)bounds.Height-1, Color.Gray);
		}
		base.Draw();
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		if (_focused && inputEvent.Type == InputEventType.KeyPress)
		{
			var k = inputEvent.KeyboardKey;
			if (k == KeyboardKey.Backspace)
			{
				if (_cursorPosition > 0 && _value.Length >0)
				{
					_value = _value.Remove(_cursorPosition - 1, 1);
					_cursorPosition--;
					OnChange?.Invoke(new TString(_value));
				}

				inputEvent.Handle();
			}
			var c = inputEvent.Character.Value;
			if (char.IsAsciiLetterOrDigit(c))
			{
				_cursorPosition++;
				if(_cursorPosition >= _value.Length){
					_value += c.ToString();
					OnChange?.Invoke(new TString(_value));
				}else
				{
					_value = _value.Insert(_cursorPosition, c.ToString());
					OnChange?.Invoke(new TString(_value));
				}
				inputEvent.Handle();
			}
		}

		if (inputEvent.Type == InputEventType.MouseLeftDown && inputEvent.Position != null)
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				inputEvent.Manager.RequestFocus(this);
				inputEvent.Handle();
			}
		}
		base.OnInput(ref inputEvent);
	}

	public void SetValue(string s, bool immediate = false)
	{
		_value = s;
		if (!immediate)
		{
			OnChange?.Invoke(new TString(_value));
		}
	}
}