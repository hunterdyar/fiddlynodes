namespace fiddlyNodes.NodeElements;

public abstract class NodeValueProperty<T> : NodeProperty
{
	public NodeValueProperty(string propertyName, Node node) : base(propertyName, node)
	{
		
	}

	public abstract T GetValue();
	public abstract void SetValue(T value);
}