using System.Numerics;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class NumberProperty : NodeProperty, IChangeReporter<TFloat>
{
	private NumberField _number;
	private UnitField _unit;
	private Label _label;
	public TFloat Value;
	
	
	public Action<TFloat> OnChange { get; set; }

	public NumberProperty(string propertyName, Node node, PortPosition inputOrOutput) : base(propertyName, node)
	{
		PropHeight = 2;
		_label = new Label("Radius", TextPosition.Center);
		_number = new NumberField(0);
		_number.OnChange += (value) => OnValuesChange();
		_unit = new UnitField(0, 0, 20, 20);
		_unit.OnChange += (value) => OnValuesChange();
		MinWidth = propertyName.Length * Raylib.GetFontDefault().BaseSize;
		AddAndSetPort(new Port(this, inputOrOutput));
		Value = new TFloat(0);
		_number.SetValue("0", true);
		
		MinWidth = 100;
		AddChild(_unit);
		AddChild(_number);
		AddChild(_label);
	}
	
	public override void Draw()
	{
		base.Draw();

		if (InputPort == null || !InputPort.IsConnected())
		{
			_number.Draw();
			_unit.Draw();
		}
		_label.Draw();
	}

	public override void Recalculate()
	{
		base.Recalculate();

		float propHeight = _transform.WorldBounds.Size.Y / PropHeight;

		_label.Transform.LocalPosition = new Vector2(0, 0);
		_label.Transform.Size = new Vector2(_transform.Size.X, propHeight);
		
		_unit.Transform.LocalPosition = new Vector2(_transform.Size.X / 2 +2, propHeight);
		_unit.Transform.Size = new Vector2(_transform.Size.X / 2-2, propHeight);
		
		_number.Transform.LocalPosition = new Vector2(2, propHeight);
		_number.Transform.Size = new Vector2(_transform.Size.X/2 - 2, propHeight);
	}

	private void OnValuesChange()
	{
		float.TryParse(_number.TextValue, out var numberValue);
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
		if (InputPort != null && InputPort.IsConnected())
		{
			//return the first value, since only one connection should exist.
			foreach (var nodeProperty in InputPort.PropertiesFrom())
			{
				return nodeProperty.GetValue(thistleType);
			}
		}//otherwise...
		
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