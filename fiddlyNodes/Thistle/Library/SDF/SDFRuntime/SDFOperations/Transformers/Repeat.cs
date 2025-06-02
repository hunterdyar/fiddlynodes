namespace fiddlyNodes.Thistle.Library;

public class Repeat : SDFOperationBase
{
	private int _countX;
	private int _countY;
	private float _scaleX;
	private float _scaleY;
	
	public Repeat(int countX, int countY, float scaleX, float scaleY)
	{
		_countX = countX;
		_countY = countY;
		_scaleX = scaleX;
		_scaleY = scaleY;
	}

	public Repeat(int countX, int countY, float size)
	{
		_countX = countX;
		_countY = countY;
		_scaleX = size;
		_scaleY = size;
	}

	public override float Calculate(float x, float y, int a = 0, int b = 0)
	{
		if (0 == _countX || 0 == _countY)
		{
			return 0;
		}
		
		int idx = (int)(x / _scaleX);
		int idy = (int)(y / _scaleY);
		int ox = MathF.Sign(x - _scaleX * idx); // neighbor offset direction
		int oy = MathF.Sign(y - _scaleY * idy);

		float d = float.MaxValue;
		for (int j = 0; j < 2; j++)
		{
			for (int i = 0; i < 2; i++)
			{
				//https://iquilezles.org/articles/sdfrepetition/
				float ridx = (idx + i * ox);
				float ridy = (idy + j * oy);
				
				if(_countX%2!=0)
				{
					//odd
					ridx = float.Clamp(ridx, -(_countX - 1) * 0.5f, (_countX - 1) * 0.5f);
				}else
				{
					//even
					ridx = float.Clamp(ridx, -(_countX-1) * 0.5f +0.5f, (_countX - 1) * 0.5f + 0.5f) -0.5f;
				}

				if (_countY % 2 != 0)
				{
					//odd
					ridy = float.Clamp(ridy, 0, _countY - 1);
				}
				else
				{
					//even
					ridy = float.Clamp(ridy, -(_countY - 1) * 0.5f + 0.5f, (_countY - 1) * 0.5f + 0.5f) - 0.5f;
				}

				float rx = x - _scaleX * ridx;
				float ry = y - _scaleY * ridy;
				d = MathF.Min(d, CalculateFrom(rx, ry, idx, idy));
			}
		}

		return d;
	}

	public override object Clone()
	{
		return new Repeat(_countX, _countY, _scaleX, _scaleY)
		{
			Parent = Parent
		};
	}
}