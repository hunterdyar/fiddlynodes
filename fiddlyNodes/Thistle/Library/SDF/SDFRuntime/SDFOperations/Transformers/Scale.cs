namespace fiddlyNodes.Thistle.Library;

public class Scale : SDFOperationBase
{
	private readonly float _scale;

	public Scale(float s)
	{
		_scale = s;
	}

	public Scale(int s)
	{
		_scale = s;
	}


	public override float Calculate(float x, float y, int a=0, int b=0)
	{
		return CalculateFrom(x/_scale, y/_scale, a,b)*_scale;
	}

	public override object Clone()
	{
		var s = new Scale(_scale)
		{
			Parent = this.Parent
		};
		return s;
	}
	
}