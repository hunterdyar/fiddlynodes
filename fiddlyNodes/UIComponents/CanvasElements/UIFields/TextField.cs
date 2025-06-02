using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class TextField : Element, IChangeReporter<TString>
{
	public string TextValue => _textValue;
	private string _textValue = string.Empty;
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
		float letterWidth = fontSize/2;
		if (_textValue.Length > 0)
		{
			letterWidth = float.Ceiling(Raylib.MeasureText(_textValue, fontSize) / (float)_textValue.Length);
		}
		int vpad = (int)Math.Floor(bounds.Height - fontSize) / 2; //vertically center
		
		Raylib.DrawRectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, Color.RayWhite);
		Raylib.DrawRectangleLines((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, _focused ? Color.Gray : Color.Black);
		Raylib.DrawText(_textValue, (int)bounds.X, (int)bounds.Y+vpad, (int)fontSize,Color.Black);
		if (_focused)
		{
			var cursorPosition = (int)(_cursorPosition*letterWidth);
			Raylib.DrawLine((int)bounds.X+cursorPosition+2, (int)bounds.Y+1, (int)bounds.X + cursorPosition+1, (int)bounds.Y+(int)bounds.Height-3, Color.Gray);
			Raylib.DrawLine((int)bounds.X + cursorPosition+2, (int)bounds.Y + (int)bounds.Height - 3, (int)(bounds.X + cursorPosition+letterWidth+2), (int)bounds.Y + (int)bounds.Height - 3, Color.Gray);

		}
		base.Draw();
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		if (_focused && inputEvent.Type == InputEventType.KeyPress)
		{
			var k = inputEvent.KeyboardKey;
			var c = inputEvent.Character.Value;

			if (k == KeyboardKey.Backspace)
			{
				if (_cursorPosition > 0 && _textValue.Length >0)
				{
					_textValue = _textValue.Remove(_cursorPosition - 1, 1);
					_cursorPosition--;
					OnChange?.Invoke(new TString(_textValue));
				}

				inputEvent.Handle();
			}else if (k == KeyboardKey.Left)
			{
				if (_cursorPosition > 0)
				{
					_cursorPosition--;
				}
				inputEvent.Handle();
			}else if (k == KeyboardKey.Right)
			{
				if (_cursorPosition < _textValue.Length)
				{
					_cursorPosition++;
				}
				inputEvent.Handle();
			}else if (IsValidInputCharacter(c))
			{
				_cursorPosition++;
				if(_cursorPosition >= _textValue.Length){
					_textValue += c.ToString();
					OnChange?.Invoke(new TString(_textValue));
				}else
				{
					_textValue = _textValue.Insert(_cursorPosition, c.ToString());
					OnChange?.Invoke(new TString(_textValue));
				}
				inputEvent.Handle();
			}
			
			
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
		_textValue = s;
		_cursorPosition = _textValue.Length;
		if (!immediate)
		{
			OnChange?.Invoke(new TString(_textValue));
		}
	}
}