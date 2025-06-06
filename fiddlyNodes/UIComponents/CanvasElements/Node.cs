﻿using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using fiddlyNodes.NodeElements;
using Raylib_cs;

namespace fiddlyNodes;

public abstract class Node : GridCanvasElement
{
	public static string DisplayName { get; }
	public static string[] Aliases { get; }
	public string UID
	{
		get => _uid;
		set => _uid = value;
	}

	protected string _uid;
	private bool _dragging = false;
	protected string _title;

	private List<NodeProperty> _nodeProperties = new List<NodeProperty>();
	private float _propertyUnitPercentage;
	private Vector2 _dragStartPosition;
	public GridCanvas Grid => _grid;

	private GridCanvas _grid;
	public Node(int x, int y, int width, int height, GridCanvas grid) : base(x, y, width, height, grid)
	{
		_title = "Node";
		int minWidth = _title.Length * Raylib.GetFontDefault().BaseSize;//baseSize is height not width but... whatever shh.
		_transform.SetSize(float.Max(width, minWidth),height);
		_grid = grid;
		grid.AddNode(this);
		_uid = NodeFactory.GetUniqueID();
	}

	public void AddProperties(params NodeProperty[] properties)
	{
		foreach (NodeProperty property in properties)
		{
			_nodeProperties.Add(property);
			AddChild(property);
			var g = property.GetPath(false);
			
		}
		Recalculate();
	}
	
	protected void Recalculate()
	{
		float propPadding = UISettings.Active.PropertyPadding;
		float y = UISettings.Active.PropertyBaseHeight+propPadding;//+1, room for title text.
		float width = _transform.Size.X;
		width = float.Max(width, _nodeProperties.Max(x=>x.MinWidth));

		foreach (NodeProperty nodeProperty in _nodeProperties)
		{
			nodeProperty.Transform.ScaleWithParent = false;//prevent cascading updates
			y += propPadding;
			nodeProperty.Transform.LocalPosition = new Vector2(0, y);
			float localHeight = UISettings.Active.PropertyBaseHeight * nodeProperty.PropHeight;
			nodeProperty.Transform.Size = new Vector2(width, localHeight);
			y += localHeight;
			nodeProperty.Recalculate();
		}
		
		_transform.Size = new Vector2(width,y);
		
		//turn scaling back on after we updated our size.
		foreach (NodeProperty nodeProperty in _nodeProperties)
		{
			nodeProperty.Transform.ScaleWithParent = true;
		}
		
		_propertyUnitPercentage = UISettings.Active.PropertyBaseHeight/y;
	}
	
	public override void Draw()
	{
		if (!_grid.Transform.OverlapsTransform(_transform)) return;
		
		var bounds = _transform.WorldBounds;
		float propertyHeight = _propertyUnitPercentage * _transform.Size.Y;
		Raylib.DrawRectangleRounded(bounds, UISettings.Active.NodeBorderRoundness, 3, UISettings.Active.NodeBGColor);
		Raylib.DrawRectangleRoundedLinesEx(bounds, UISettings.Active.NodeBorderRoundness, 3, UISettings.Active.NodeBorderThickness, UISettings.Active.NodeBorderColor);

		Raylib.DrawText(_title, (int)bounds.X + 5, (int)bounds.Y, (int)float.Floor(propertyHeight),
			UISettings.Active.TextColor);

		base.Draw();
	}
	

	public override void OnInput(ref InputEvent inputEvent)
	{
		if (_dragging && inputEvent.Type == InputEventType.MouseLeftUp)
		{
			_dragging = false;
			inputEvent.Handle();
			if (_dragStartPosition != _transform.LocalPosition)
			{
				var moveCommand = new MoveNodesCommand(this, _dragStartPosition, _transform.LocalPosition);
				Program.Commands.AddAndExecute(moveCommand);
			}
		}else if (inputEvent is { Type: InputEventType.MouseLeftDown, Position: not null})
		{
			if (_transform.ContainsPoint(inputEvent.Position.Value))
			{
				if (_focused && !_dragging)
				{
					return;
				}
				
				if (!_dragging)
				{
					_dragging = true;
					Program.Input.RequestFocus(this);
					_dragStartPosition = this.Transform.LocalPosition;
					inputEvent.Handle();
				}
			}

		}else if (inputEvent is { Type: InputEventType.MouseMove, Delta: not null })
		{
			if(_dragging){
				_transform.Translate(inputEvent.Delta.Value);
				//inputEvent.Handle();
			}
		}else if (inputEvent is { Type: InputEventType.Hover})
		{
			inputEvent.Manager.RequestHover(this);
			inputEvent.Handle();
		}else if (inputEvent is { Type: InputEventType.KeyPress })
		{
			if (_focused && !_dragging)
			{
				if (inputEvent.KeyboardKey == KeyboardKey.Backspace || inputEvent.KeyboardKey == KeyboardKey.Delete)
				{
					inputEvent.Manager.ClearFocus();
					Delete();
				}
			}
		}
	}

	/// <summary>
	/// This should return any properties that will have their value serialized and deserialized NOT via wires.
	/// so input-only properties will be skipped, and parent properties should return their children.
	/// </summary>
	public IEnumerable<NodeProperty> GetPropertiesForSerialization()
	{
		//each prop and it's children.
		foreach (NodeProperty property in _nodeProperties)
		{
			foreach (NodeProperty nodeProperty in property.GetProperties())
			{
				yield return nodeProperty;
			}
		}
	}
	private void Delete()
	{
		foreach (NodeProperty nodeProperty in _nodeProperties)
		{
			nodeProperty.ClearAllConnections();
		}
		_grid.RemoveNode(this);
	}

	public void SetNodePropertyByPath(string path, string value)
	{
		var nodeProp = _nodeProperties.Find(x=>x.GetPath(false) == path);
		if (nodeProp != null)
		{
			nodeProp.SetValueFromString(value);
		}
	}

	public NodeProperty GetPropertyByPath(string subpath)
	{
		//search primary properties.
		var np = _nodeProperties.Find(x => x.PropertyName == subpath);
		if (np != null)
		{
			return np;
		}
		//okay, search sub properties now.
		var y = GetPropertiesForSerialization().ToList();
		return y.Find(x=>x.GetPath(false) == subpath);
	}
}