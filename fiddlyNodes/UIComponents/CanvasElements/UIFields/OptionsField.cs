using fiddlyNodes.Thistle.Library;

namespace fiddlyNodes;

public class OptionsField<T> : Element where T : TreeBaseObject
{
	public T Selected;
	public List<T> Options;
	
	public OptionsField(int x, int y, int width, int height) : base(x, y, width, height)
	{
		
	}
	
	
}