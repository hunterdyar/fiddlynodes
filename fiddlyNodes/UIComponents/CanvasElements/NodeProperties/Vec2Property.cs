using System.Numerics;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class Vec2Property : NodeProperty<TVec2>
{
	private NumberProperty _x;
	private NumberProperty _y;

	private UnitField _unit;
	private Label _label;
	public TVec2 Value;
	
	public Action<TVec2> OnChange { get; set; }
	
	public Vec2Property(string propertyName, Node node, PortPosition inputOrOutput) : base(propertyName, node)
	{
		Serialize = true;
		PropHeight = 5;//label, x, y.
		_label = new Label("Vector 2", TextPosition.Center);
		_x = new NumberProperty("X", node, PortPosition.Input, this);
		_x.OnChange += (value) => OnValuesChange();
		_y = new NumberProperty("Y", node, PortPosition.Input, this);
		
		_y.OnChange += (value) => OnValuesChange();
		_unit = new UnitField(0, 0, 20, 20);
		_unit.OnChange += (value) => OnValuesChange();
		
		MinWidth = propertyName.Length * Raylib.GetFontDefault().BaseSize;
		
		//get the combined vec2.
		if (inputOrOutput == PortPosition.Output)
		{
			AddAndSetPort(new Port(this, inputOrOutput));
		}

		Value = new TVec2(Vector2.Zero);
		// _x.SetValue("0", true);
		// _y.SetValue("0", true);
		
		MinWidth = 100;
		AddChild(_unit);
		AddChild(_x);
		AddChild(_y);
		AddChild(_label);
	}
	
	public override void Draw()
	{
		base.Draw();

		if (InputPort == null || !InputPort.IsConnected())
		{
			_x.Draw();
			_y.Draw();
		//	_unit.Draw();
		}
		_label.Draw();
	}

	public override void Recalculate()
	{
		base.Recalculate();

		float propHeight = _transform.WorldBounds.Size.Y / PropHeight;

		_label.Transform.LocalPosition = new Vector2(0, 0);
		_label.Transform.Size = new Vector2(_transform.Size.X, propHeight);
		
		_x.Transform.LocalPosition = new Vector2(2, propHeight);
		_x.Transform.Size = new Vector2(_transform.Size.X, propHeight*2);

		_y.Transform.LocalPosition = new Vector2(2, propHeight*3);
		_y.Transform.Size = new Vector2(_transform.Size.X, propHeight*2);
		_x.Recalculate();
		_y.Recalculate();
	}

	/// <summary>
	/// Internal, called to update number when various values change.
	/// </summary>
	private void OnValuesChange()
	{
		Value = new TVec2(new Vector2(_x.Value.Value, _y.Value.Value));
		OnChange?.Invoke(Value);
	}
	
	public override TVec2 GetValue()
	{
		if (InputPort != null && InputPort.IsConnected())
		{
			//return the first value, since only one connection should exist.
			foreach (var nodeProperty in InputPort.PropertiesFrom())
			{
				if (nodeProperty is NodeProperty<TVec2> nodeProp)
				{
					return nodeProp.GetValue();
				}
				else
				{
					throw new InvalidCastException("Invalid node connection");
				}
			}
		}//otherwise...
		
		return Value;
	}

	public override IEnumerable<NodeProperty> GetProperties()
	{
		yield return _x;
		yield return _y;
	}

	public override string ToString()
	{
		return _x.ToString()+","+_y.ToString();
	}

	public override void SetValueFromString(string value)
	{
		var val = value.Split(',');
		_x.SetValueFromString(val[0]);
		_y.SetValueFromString(val[1]);
	}
}