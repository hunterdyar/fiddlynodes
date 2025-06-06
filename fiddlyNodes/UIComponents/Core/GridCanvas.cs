using System.Numerics;
using Raylib_cs;

namespace fiddlyNodes;

public class GridCanvas : Element
{
	public int gridDensity = 10;
	public Vector2 offset;
	public float zoomValue;
	private bool _dragging;
	private ElementContainer _gridTransform;
	public WireManager WireManager => _wireManager;
	private WireManager _wireManager;
	private readonly List<Node> _allNodes = new List<Node>();
	public GridCanvas(int x, int y, int width, int height) : base(x, y, width, height)
	{
		_wireManager = new WireManager();
		this.gridDensity = 10;
		this.zoomValue = 1;
		this.offset = new Vector2(0, 0);
		_gridTransform = new ElementContainer(x, y, 1, 1);
		_gridTransform.Transform.SetPassthrough(true);
		AddChild(_gridTransform);
	}

	public List<Node> GetAllNodes() => _allNodes;

	public void AddNode(Node node)
	{
		_allNodes.Add(node);
		AddChild(node);
	}

	public void RemoveNode(Node node)
	{
		if(_allNodes.Contains(node))
		{
			_allNodes.Remove(node);
		}

		_gridTransform.Transform.RemoveChild(node.Transform);
	}
	
	public override void AddChild(Element element)
	{
		
		if (element == _gridTransform)
		{
			base.AddChild(element);
		}
		else
		{
			//oops let me just slip _gridTransform in there
			_gridTransform.AddChild(element);
		}
	}

	public override void Draw()
	{
		if (zoomValue == 0)
		{
			throw new Exception("Cannot zoom to value of zero.");
		};
		zoomValue = _gridTransform.Transform.Size.Y;
		var visibleResolution = gridDensity * zoomValue;
		var worldBoundingBox = _transform.WorldBounds;
		for (var x = worldBoundingBox.X + (offset.X*zoomValue) % visibleResolution; x < worldBoundingBox.X+worldBoundingBox.Width; x += visibleResolution)
		{
			for (var y = worldBoundingBox.Y + (offset.Y*zoomValue) % visibleResolution; y < worldBoundingBox.Y+worldBoundingBox.Height; y += visibleResolution)
			{
				if (zoomValue <= 1)
				{
					Raylib.DrawPixel((int)(x), (int)(y), UISettings.Active.GridDotColor);
				}
				else
				{
					Raylib.DrawCircle((int)(x), (int)(y),zoomValue, UISettings.Active.GridDotColor);
				}
			}
		}

		_wireManager.Draw(this._transform);
		
		Raylib.DrawRectangleLinesEx(worldBoundingBox, 1, Color.Gray);
		//call base after so children draw on top.
		base.Draw();
	}

	public override void OnInput(ref InputEvent inputEvent)
	{
		_wireManager.PassInput(ref inputEvent);
		if (inputEvent.Handled)
		{
			return;
		}
		
		base.OnInput(ref inputEvent);
		if (inputEvent.Type == InputEventType.MouseScroll)
		{
			var delta = inputEvent.Delta.Value;
			
			_gridTransform.Transform.ScaleAroundPoint(inputEvent.Position.Value, 1+(0.2f*delta.Y));
			
			inputEvent.Handle();
		}

		if (inputEvent.Type == InputEventType.MouseLeftDown)
		{
			inputEvent.Manager.ClearFocus();
			inputEvent.Handle();
			return;
		}

		if (inputEvent.Type == InputEventType.MouseMiddleDown)
		{
			if (!_dragging)
			{
				_dragging = true;
				inputEvent.Handle();
			}
		}

		if (inputEvent.Type == InputEventType.MouseMiddleUp)
		{
			if (_dragging)
			{
				_dragging = false;
				inputEvent.Handle();
			}
		}

		if (inputEvent.Type == InputEventType.MouseMove)
		{
			if (_dragging)
			{
				_gridTransform.Transform.Translate(inputEvent.Delta.Value);
				inputEvent.Handle();
			}
		}

		if (inputEvent is { Type: InputEventType.Hover})
		{
			if (!inputEvent.Handled)
			{
				inputEvent.Manager.ClearHover();
				inputEvent.Handle();
			}
			return;
		}

	}


	
}