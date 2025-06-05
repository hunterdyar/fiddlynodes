using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class TextInputHandler
{
	public int CursorPos => _cursorPosition;
	private int _cursorPosition;
	
	public string Value => _value;
	private string _value = string.Empty;
	private Func<char, bool> _isValid;
	public Action<string> OnChange { get; set; }

	public TextInputHandler()
	{
		//default
		_isValid = char.IsAsciiLetterOrDigit;
	}
	public TextInputHandler(Func<char, bool> isValid)
	{
		_isValid = isValid;
	}

	private void Backspace(bool withControl = false, bool immediate = false)
	{
		bool changed = false;

		int removeCount = !withControl ? 1 : _cursorPosition;
		
		while(removeCount > 0 && _value.Length > 0)
		{
			_value = _value.Remove(_cursorPosition - 1, 1);
			_cursorPosition--;
			changed = true;
			removeCount--;
		}

		if (!immediate && changed)
		{
			OnChange?.Invoke(_value);
		}
		
	}
	public void OnInput(ref InputEvent inputEvent)
	{
		if (inputEvent.Type == InputEventType.KeyPress)
		{
			var k = inputEvent.KeyboardKey;
			var c = inputEvent.Character.Value;
			if (k == KeyboardKey.Backspace)
			{
				bool ctrl = Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl);
				Backspace(ctrl, false);
				inputEvent.Handle();
			}
			else if (k == KeyboardKey.Left)
			{
				if (_cursorPosition > 0)
				{
					_cursorPosition--;
				}

				inputEvent.Handle();
			}
			else if (k == KeyboardKey.Right)
			{
				if (_cursorPosition < _value.Length)
				{
					_cursorPosition++;
				}

				inputEvent.Handle();
			}
			else if (_isValid(c))
			{
				_cursorPosition++;
				if (_cursorPosition >= _value.Length)
				{
					_value += c.ToString();
					OnChange?.Invoke(_value);
				}
				else
				{
					_value = _value.Insert(_cursorPosition, c.ToString());
					OnChange?.Invoke(_value);
				}

				inputEvent.Handle();
			}
		}

		
	}

	public void SetValue(string s, bool immediate = false)
	{
		_value = s;
		_cursorPosition = _value.Length;
		if (!immediate)
		{
			OnChange?.Invoke(_value);
		}
	}
	
}