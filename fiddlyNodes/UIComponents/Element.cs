using System.Drawing;
using System.Numerics;
using Microsoft.VisualBasic;
using Rectangle = Raylib_cs.Rectangle;

namespace fiddlyNodes;

public abstract class Element : IHoverable
{
	public Element? Parent;
	public RectTransform Transform => _transform;
	protected RectTransform _transform;
	
	protected bool _focused { get; private set; }
	protected bool _hovering { get; private set; }
	public Element(int x, int y, int width, int height)
	{
		_transform = new RectTransform(new Vector2(x,y),new Vector2(width,height), this);
	}
	
	public virtual void Draw()
	{
		foreach (var child in _transform.Children)
		{
			child.Element.Draw();
		}
	}
	
	public virtual void AddChild(Element element)
	{
		_transform.AddChild(element._transform);
	}

	public virtual bool OverlapsPoint(Vector2 pos)
	{
		return _transform.ContainsPoint(pos);
	}

	
	public virtual void OnInput(ref InputEvent inputEvent)
	{
		
	}

	public virtual void OnGainFocus()
	{
		_focused = true;
	}
	public virtual void OnLoseFocus()
	{
		_focused = false;
	}
	public virtual void OnGainHover()
	{
		_hovering = true;
	}
	public void OnLoseHover()
	{
		_hovering = false;
	}
}