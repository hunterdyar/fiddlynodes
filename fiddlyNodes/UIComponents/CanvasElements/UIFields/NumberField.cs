using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes;

public class NumberField : TextField
{
	//todo: fix this the input class needs a seperate thing for AddLetter
	public string TextValue => _text.Value;
	public float Value
	{
		get => Math.Clamp(float.Parse(_text.Value), minValue, maxValue);
		set => _text.Value = value.ToString();
	}

	public float minValue = float.NegativeInfinity;
	public float maxValue = float.PositiveInfinity;
	public NumberField(float minValue = float.NegativeInfinity, float maxValue = float.PositiveInfinity) : base(0, 0, 0, 0)
	{
		this.maxValue = maxValue;
		this.minValue = minValue;
		_text = new TextInputHandler(IsValidNumberInputCharacter);
		_text.OnChange += ChangeHandler;
	}

	private void ChangeHandler(string s)
	{
		OnChange?.Invoke(new TString(s));
	}

	protected bool IsValidNumberInputCharacter(char c)
	{
		return char.IsAsciiDigit(c) || c == '.';
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		base.OnInput(ref inputEvent);
		if (inputEvent.Type == InputEventType.MouseScroll)
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				//todo: gross and stupid for string to be the backing property.
				float value = Value;
				value = value + inputEvent.Delta.Value.Y;
				SetValue(Math.Clamp(value, minValue, maxValue).ToString());
				inputEvent.Handle();
			} 
		}
	}
}