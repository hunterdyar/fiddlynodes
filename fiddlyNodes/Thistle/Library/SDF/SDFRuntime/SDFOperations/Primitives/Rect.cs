﻿namespace fiddlyNodes.Thistle.Library;

public class Rect : SDFOperationBase
{
	private float _width;
	private float _height;
	public Rect(float size)
	{
		this._width = size;
		this._height = size;
	}

	public Rect(float width, float height)
	{
		this._width = width;
		this._height = height;
	}

	public override float Calculate(float x, float y, int a=0, int b=0)
	{
		float dx = MathF.Abs(x) - _width;
		float dy = MathF.Abs(y) - _height;


		float cx = MathF.Max(dx, 0);
		float cy = MathF.Max(dy, 0);
		
		//length
		return MathF.Sqrt(cx*cx+cy*cy) + MathF.Min(MathF.Max(dx, dy), 0);
	}

	public override object Clone()
	{
		return new Rect(_width, _height)
		{
			Parent = Parent
		};
	}

	public void SetSize(float size)
	{
		_width = size;
		_height = size;
	}

	public void SetSize(float width, float height)
	{
		_width = width;
		_height = height;
	}

	public void SetHeight(float value)
	{
		_height = value;
	}

	public void SetWidth(float value)
	{
		_width = value;
	}
}