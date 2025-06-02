namespace fiddlyNodes.Thistle.Library;

public abstract class TreeNativeObject<T> : TreeBaseObject
{
	public TreeNativeObject(T value)
	{
		Value = value;
	}
	public T Value { get; set; }

	public void SetValue(T value)
	{
		this.Value = value;
	}

	public T GetValue()
	{
		return Value;
	}
}