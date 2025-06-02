namespace fiddlyNodes.Thistle.Library;

//todo: this is being removed in the refactor.
public abstract class TreeBaseObject 
{
	public virtual ThistleType TType => ThistleType.tnone;
	
	public virtual void SetValue(TreeBaseObject value)
	{
		//throw new InvalidOperationException($"Can't set value of {this} to {TType}.");
	}

	//todo: move to utility class.
	public static TreeBaseObject? GetDefaultObject(ThistleType tType)
	{
		switch (tType)
		{
			case ThistleType.tint:
				return new TInt(0);
			case ThistleType.Tfloat:
				return new TFloat(0);
			case ThistleType.tstring:
				return new TString("");
			case ThistleType.tsdf:
				return new TSDF();
		}
		
		throw new Exception($"{tType} cannot automatically implement a default context.");
	}

	public abstract TreeBaseObject Clone();
}