using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public abstract class OptionsField<T> : Element, IChangeReporter<T> where T : TreeBaseObject
{
	public T Selected;
	protected int _selectedIndex;
	public List<T> Options;
	public Action<T> OnChange { get; set; }
	
	public OptionsField(int x, int y, int width, int height) : base(x, y, width, height)
	{
		_selectedIndex = 0;
	}

	public override void Draw()
	{
		var w = Transform.WorldBounds;
		var text = Selected != null ? Selected.ToString() : "<>";

		int fontSize = (int)(w.Height * 0.9f);
		float letterWidth = 0;
		if (text.Length > 0)
		{
			letterWidth = float.Ceiling(Raylib.MeasureText(text, fontSize) / (float)text.Length);
		}

		int vpad = (int)Math.Floor(w.Height - fontSize) / 2; //vertically center
		
		Raylib.DrawText(text, (int)w.X, (int)w.Y + vpad, fontSize, Color.DarkPurple);
		base.Draw();
	}

	public void Select()
	{
		if (Options.Count == 0)
		{
			return;
		}

		//_selectedIndex = Options.IndexOf(Selected);
		_selectedIndex = (_selectedIndex + 1) % Options.Count;
		Selected = Options[_selectedIndex];
		OnChange?.Invoke(Selected);
	}
	public override void OnInput(ref InputEvent inputEvent)
	{
		base.OnInput(ref inputEvent);
		if (inputEvent.Type == InputEventType.MouseLeftDown)
		{
			Select();
		}
	}

}