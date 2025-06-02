using fiddlyNodes.Thistle.Library;
namespace fiddlyNodes.Thistle;

public class TSDF : TreeBaseObject
{
	//todo: Assignable Object Fields. How to handle with generic?
	public TColor Color = new TColor(Raylib_cs.Color.Black);
	public override ThistleType TType => ThistleType.tsdf;

	private SDFOperationBase? _leaf;

	public void AddOperation(SDFOperationBase operation)
	{
		//todo: test if we need to to clone for when inside of loops... once we have loops
		//operation = (SDFOperationBase)operation.Clone();
		operation.Parent = _leaf;
		_leaf = operation;
	}

	public float GetSDFValue(int x, int y)
	{
		if (_leaf == null)
		{
			return 0;
		}

		float f = _leaf.Calculate(x, y);
		return f;
	}

	public override string ToString()
	{
		return "SDF: " + string.Join("\n", _leaf.ToString()) + "\n";
	}

	public override TreeBaseObject Clone()
	{
		return new TSDF()
		{
			_leaf =  this._leaf
		};
	}
}