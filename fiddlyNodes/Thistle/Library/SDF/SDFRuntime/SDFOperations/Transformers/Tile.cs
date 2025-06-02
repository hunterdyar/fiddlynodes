namespace fiddlyNodes.Thistle.Library;

/// <summary>
/// Tile is repeat but infinite.
/// </summary>
public class Tile : SDFOperationBase
{
	private readonly float _scale;

	public Tile(float scale)
	{
		_scale = scale;
	}

	public override float Calculate(float x, float y, int idx = 0, int idy=0)
	{
		idx = (int)(x / _scale);
		idy = (int)(y / _scale);
		int ox = MathF.Sign(x - _scale * idx); // neighbor offset direction
		int oy = MathF.Sign(y - _scale * idy);
		
		float d = float.MaxValue;
		for (int j = 0; j < 2; j++)
		for (int i = 0; i < 2; i++)
		{
			//https://iquilezles.org/articles/sdfrepetition/
			float rx = x - _scale * (idx + i * ox);
			float ry = y - _scale * (idy + j * oy);
			d = MathF.Min(d, CalculateFrom(rx,ry,idx,idy));
		}

		return d;
	}

	public override object Clone()
	{
		var tile = new Tile(_scale)
		{
			Parent = this.Parent
		};
		return tile;
	}
}