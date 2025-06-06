﻿using System.Numerics;
using fiddlyNodes;
using fiddlyNodes.NodeElements;
using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

public class StringProperty : NodeProperty<TString>
{
	private TextField _field;
	//todo: cache, etc.
	private TString Value => new TString(_field.TextValue);
	public StringProperty(string propertyName, Node node) : base(propertyName, node)
	{
		Serialize = true;
		_field = new TextField(0, 0, 20, 12);
		AddChild(_field);
		MinWidth = propertyName.Length * Raylib.GetFontDefault().BaseSize;
		_transform.ScaleWithParent = true;
		AddAndSetPort(new Port(this, PortPosition.Output));
	}

	public override void Recalculate()
	{
		base.Recalculate();
		
		_field.Transform.LocalPosition = new Vector2(_transform.Size.X / 2, 0);
		_field.Transform.Size = new Vector2(_transform.Size.X / 2, _transform.Size.Y);
	}

	public override void Draw()
	{
		base.Draw();
		int hpad = 5; //get port size/2+pad.

		if (InputPort != null)
		{
			//.5 for port radius + .1 extra
			hpad = (int)(InputPort.Transform.Size.X * 0.6);
		}
		
		var w = _transform.WorldBounds;
		int fontSize = (int)(w.Height * 0.9f);
		int vpad = (int)Math.Floor(w.Height - fontSize) / 2; //vertically center

		Raylib.DrawText(propertyName, (int)w.X + hpad, (int)w.Y + vpad, fontSize, Color.Black);
		_field.Draw();
	}

	public override TString GetValue()
	{
		return Value;
	}

	public override string ToString()
	{
		return Value.Value;
	}

	public override void SetValueFromString(string value)
	{
		Value.Value = value;
	}
}