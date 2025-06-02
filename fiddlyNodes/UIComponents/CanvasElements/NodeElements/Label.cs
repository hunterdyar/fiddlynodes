using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class Label : Element
{
	public string LabelText => _labelText;
	private string _labelText;
	public TextPosition TextPosition;
	
	public Label(string label, TextPosition textPosition) : base(0, 0, 0, 0)
	{
		SetLabel(label, textPosition);
	}

	public void SetLabel(string label, TextPosition textPosition)
	{
		_labelText = label;
		TextPosition = textPosition;
	}
	override public void Draw()
	{
		base.Draw();
		var w = _transform.WorldBounds;
		int fontSize = (int)(w.Height * 0.9f);
		var letterWidth = 0f;
		var totalWidth = 0f;
		if (LabelText.Length > 0)
		{
			totalWidth = Raylib.MeasureText(_labelText, fontSize);
			letterWidth = float.Ceiling(totalWidth / (float)_labelText.Length);
		}
		int vpad = (int)Math.Floor(w.Height - fontSize) / 2; //vertically center
		int xpos = 0;
		switch (TextPosition)
		{
			case TextPosition.Left:
				xpos = (int)w.X;
				break;
			case TextPosition.Right:
				xpos = (int)(w.X + w.Width - totalWidth);
				break;
			case TextPosition.Center:
				xpos = (int)(w.X + w.Width / 2 - totalWidth/2);
				break;
		}
		Raylib.DrawText(LabelText, xpos, (int)w.Y + vpad, fontSize, Color.Black);
	}
}