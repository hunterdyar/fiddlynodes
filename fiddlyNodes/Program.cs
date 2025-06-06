using System.ComponentModel;
using System.Numerics;
using fiddlyNodes.Nodes;
using Raylib_cs;

namespace fiddlyNodes;

public class Program
{
	
	public readonly static CommandSystem Commands = new CommandSystem();
	public readonly static InputManager Input = new InputManager();
	public readonly static SaveManager SaveManager = new SaveManager();
	public readonly static NodeFinder NodeFinder = new NodeFinder();
	
	public readonly static Element Hierarchy = new ElementContainer(0,0,640,480);
	public static OutputContainer OutputContainer;
	public static RectTransform Root => Hierarchy.Transform;
	public static OutputNode PrimaryOutputNode;
	public static GridCanvas GridCanvas;
	
	private static int width = 800;
	private static int height = 600;
	public static void Main()
	{
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(width,height, "fiddly widdly nodily wodily");
		int canvasWidth = (int)(width * .7f);
		int outputWidth = width - canvasWidth;
		GridCanvas = new GridCanvas(0,0,canvasWidth,height);
		OutputContainer = new OutputContainer(canvasWidth,0,outputWidth,outputWidth);
		Hierarchy.AddChild(GridCanvas);
		Hierarchy.AddChild(OutputContainer);
		
		//test data
		PrimaryOutputNode = new OutputNode(300, 100, GridCanvas);
		var node2 = new FloatNode(40, 20, GridCanvas);
		var v = new Vec2Node(80, 40,  GridCanvas);
		var t = new TranslateNode(500,500, GridCanvas);
		var node3 = new CircleNode(40, 60, GridCanvas);
		Console.WriteLine(NodeFactory.SerializeNode(v));
		Console.WriteLine("---");
		Console.WriteLine(NodeFactory.SerializeNode(t));
		while (!Raylib.WindowShouldClose())
		{
			//first do inputs and controls.
			Input.Tick();
			Raylib.ClearBackground(UISettings.Active.BGColor);
			
			//draw.
			Raylib.BeginDrawing();
			Hierarchy.Draw();
			Input.Draw();
			NodeFinder.Draw();
			
			//Input.DebugDraw();
			Raylib.EndDrawing();

			if (Raylib.IsWindowResized())
			{
				Hierarchy.Transform.Size = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
			}
		}
	}
}