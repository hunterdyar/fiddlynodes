using System.Security.AccessControl;
using fiddlyNodes.Nodes;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes;

public class NodeFinder
{
	public Dictionary<string, NodeCreationItem> nodeItems = new Dictionary<string, NodeCreationItem>();
	private List<string> _terms;
	private bool _showWindow;

	private TextInputHandler _text;
	
	private int _selectedIndex = 0;
	private int  _optionHeight = 16;
	
	private List<NodeCreationItem> _searchItems = new List<NodeCreationItem>();
	public NodeFinder()
	{
		PopulateNodeItems();
		//for testing
		_text = new TextInputHandler(x=> char.IsAsciiLetter(x) || x == ' ');
		_text.OnChange += x => OnQueryChange();
	}

	#region Window Things

	public void Draw()
	{
		if (!_showWindow)
		{
			return;
		}
		//bounds
		var w = Raylib.GetScreenWidth()/2;
		var h = Raylib.GetScreenHeight()/2;
		var x = w - w / 2;
		var y = h - h / 2;
		
		h = int.Min(h, (_searchItems.Count + 1) * _optionHeight);
		
		Raylib.DrawRectangle(x,y,w,h,Color.Beige);
		Raylib.DrawRectangleLines(x,y,w,_optionHeight,Color.Black);
		Raylib.DrawText(_text.Value, x+2, y+2, _optionHeight-2, Color.Black);

		for (int i = 0; i < _searchItems.Count; i++)
		{
			var c = _selectedIndex == i ? Color.LightGray : Color.Beige;
			var dy = y + (i + 1) * _optionHeight;
			Raylib.DrawRectangle(x,dy,w,_optionHeight,c);
			Raylib.DrawRectangleLines(x, dy, w, _optionHeight, Color.Gray);
			Raylib.DrawText(_searchItems[i].name, x + 2, dy + 2, _optionHeight - 2, Color.Black);
		}
	}

	private void OpenWindow()
	{
		_showWindow = true;
	}
	private void CloseWindow()
	{
		_showWindow = false;
		//close but don't fire change event. This way, when you open nodefinder again, it has the previous items selected until you start typing.
		_text.SetValue(string.Empty,true);
	}

	public void OnInput(ref InputEvent inputEvent)
	{
		if (inputEvent.Type != InputEventType.KeyPress)
		{
			//for now, keyboard only. type and slap enter.
			return;
		}
		if (!_showWindow)
		{
			if (inputEvent.KeyboardKey == KeyboardKey.Tab)
			{
				OpenWindow();
				inputEvent.Handle();
				return;
			}
		}
		else
		{
			//pass typing input to text
			//enter to select highlighted node
			//up/down to move index.
			if (inputEvent.KeyboardKey == KeyboardKey.Enter)
			{
				if (_selectedIndex >= 0 && _selectedIndex < _searchItems.Count)
				{
					var selected = _searchItems[_selectedIndex];
					selected.CreateNodeFunction.Invoke();
				}
				CloseWindow();
				inputEvent.Handle();
			}else if (inputEvent.KeyboardKey == KeyboardKey.Escape || inputEvent.KeyboardKey == KeyboardKey.Tab)
			{
				CloseWindow();
				inputEvent.Handle();
			}else if (inputEvent.KeyboardKey == KeyboardKey.Up)
			{
				MoveSelectedUp();
				inputEvent.Handle();
			}
			else if (inputEvent.KeyboardKey == KeyboardKey.Down)
			{
				MoveSelectedDown();
				inputEvent.Handle();
			}
			else
			{
				_text.OnInput(ref inputEvent);
			}
		}
	}

	#endregion

	#region Search and Filter

	private void OnQueryChange()
	{
		_searchItems = FindMatchingItems(_text.Value.ToLowerInvariant(), 10);
		_selectedIndex = int.Min(_searchItems.Count-1,_selectedIndex);
	}

	private void MoveSelectedDown()
	{
		_selectedIndex++;
		_selectedIndex = Math.Min(_selectedIndex, _searchItems.Count - 1);
	}

	//Zero is the top of our list.
	private void MoveSelectedUp()
	{
		_selectedIndex--;
		_selectedIndex = Math.Max(_selectedIndex,0);
	}
	public List<NodeCreationItem> FindMatchingItems(string search, int count = 10)
	{
		return nodeItems.OrderBy(x => FuzzyUtility.DLDistance(search, x.Key)).Select(x => x.Value).Distinct().Take(count).ToList();
	}

	private void PopulateNodeItems()
	{
		nodeItems.Add("circle", new NodeCreationItem()
		{
			//todo: There is probably a clever way to make this func with generics and some <T>.
			//Probably related to a 'builder' or 'factory' pattern.
			CreateNodeFunction = () =>
			{
				var mp = Raylib.GetMousePosition();
				return new CircleNode((int)mp.X, (int)mp.Y, 0, 0, Program.GridCanvas);
			},
			name = "Circle",
		});
		nodeItems.Add("translate", new NodeCreationItem()
		{
			CreateNodeFunction = () =>
			{
				var mp = Raylib.GetMousePosition();
				return new TranslateNode((int)mp.X, (int)mp.Y, 0, 0, Program.GridCanvas);
			},
			name = "Translate",
		});
		nodeItems.Add("float", new NodeCreationItem()
		{
			CreateNodeFunction = () =>
			{
				var mp = Raylib.GetMousePosition();
				return new FloatNode((int)mp.X, (int)mp.Y, 0, 0, Program.GridCanvas);
			},
			name = "Float",
		});
		var rectItem = new NodeCreationItem()
		{
			CreateNodeFunction = () =>
			{
				var mp = Raylib.GetMousePosition();
				return new RectNode((int)mp.X, (int)mp.Y, 0, 0, Program.GridCanvas);
			},
			name = "Rectangle",
		};
		//multiple aliases for Rect.
		nodeItems.Add("square", rectItem);
		nodeItems.Add("rect",rectItem);
		nodeItems.Add("rectangle", rectItem);

		nodeItems.Add("stroke", new NodeCreationItem()
		{
			CreateNodeFunction = () =>
			{
				var mp = Raylib.GetMousePosition();
				return new StrokeNode((int)mp.X, (int)mp.Y, Program.GridCanvas);
			},
			name = "Stroke",
		});
		
		_terms = nodeItems.Keys.ToList();
	}
	
	#endregion
	
}