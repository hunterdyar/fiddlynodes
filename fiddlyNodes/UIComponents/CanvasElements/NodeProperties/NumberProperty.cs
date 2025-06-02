using System.Numerics;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class NumberProperty : NodeProperty, IChangeReporter<TFloat>
{
	private TextField _number;
	private UnitField _unit;
	public TFloat Value;
	public Action<TFloat> OnChange { get; set; }

	public NumberProperty(string propertyName, Node node) : base(propertyName, node)
	{
		_number = new TextField(0,0,20,20);
		_number.OnChange += (value) => OnValuesChange();
		_unit = new UnitField(0, 0, 20, 20);
		_unit.OnChange += (value) => OnValuesChange();
		MinWidth = propertyName.Length * Raylib.GetFontDefault().BaseSize;
		AddAndSetPort(new Port(this,PortPosition.Input));
		Value = new TFloat(0);
		_number.SetValue("0", true);
		MinWidth = 100;
		AddChild(_unit);
		AddChild(_number);
	}

	public override void Draw()
	{
		DrawPropertyName();
		if (!InputPort.IsConnected())
		{
			_number.Draw();
		}

		_unit.Draw();
		base.Draw();
	}

	public override void Recalculate()
	{
		base.Recalculate();
		
		_unit.Transform.LocalPosition = new Vector2(_transform.Size.X / 2, 0);
		_unit.Transform.Size = new Vector2(_transform.Size.X / 2, _transform.Size.Y);
	}

	private void OnValuesChange()
	{
		float.TryParse(_number.Value, out var numberValue);
		switch (_unit.Selected.Value)
		{
			case Unit.Pixels:
				Value = new TFloat(numberValue);
				break;
			case Unit.PercentHeight:
				Value = new TFloat(numberValue / 100f * Program.OutputContainer.OutputHeight);
				break;
			case Unit.PercentWidth:
				Value = new TFloat(numberValue / 100f * Program.OutputContainer.OutputWidth);
				break;
		}
		Value = new TFloat(numberValue);
		OnChange?.Invoke(Value);
	}

	public override TreeBaseObject GetValue(ThistleType thistleType)
	{
		if (InputPort.IsConnected())
		{
			//return the first value, since only one connection should exist.
			foreach (var nodeProperty in InputPort.PropertiesFrom())
			{
				return nodeProperty.GetValue(thistleType);
			}
		}
		if (thistleType == ThistleType.Tfloat)
		{
			return Value;
		}
		if (thistleType == ThistleType.tint)
		{
			return new TInt((int)Value.Value);
		}
		return base.GetValue(thistleType);
	}
}