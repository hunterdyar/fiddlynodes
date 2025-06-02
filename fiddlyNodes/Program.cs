using System.ComponentModel;
using System.Numerics;
using fiddlyNodes.Nodes;
using Raylib_cs;

namespace fiddlyNodes;

public class Program
{
	
	public readonly static CommandSystem Commands = new CommandSystem();
	public readonly static InputManager Input = new InputManager();
	public readonly static Element Hierarchy = new ElementContainer(0,0,640,480);
	public static OutputContainer OutputContainer;
	public static RectTransform Root => Hierarchy.Transform;
	public static OutputNode PrimaryOutputNode;
	
	private static int width = 800;
	private static int height = 600;
	public static void Main()
	{
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(width,height, "fiddly widdly nodily wodily");

		int canvasWidth = (int)(width * .7f);
		int outputWidth = width - canvasWidth;
		var gridCanvas = new GridCanvas(0,0,canvasWidth,height);
		OutputContainer = new OutputContainer(canvasWidth,0,outputWidth,outputWidth);
		Hierarchy.AddChild(gridCanvas);
		Hierarchy.AddChild(OutputContainer);
		
		//test data
		PrimaryOutputNode = new OutputNode(300, 100, 20, 20, gridCanvas);
		var node2 = new FloatNode(40, 20, 30, 30, gridCanvas);
		var node3 = new CircleNode(40, 60, 20, 20, gridCanvas);
		while (!Raylib.WindowShouldClose())
		{
			//first do inputs and controls.
			Input.Tick();
			Raylib.ClearBackground(Color.RayWhite);
			
			
			//draw.
			Raylib.BeginDrawing();
			Hierarchy.Draw();
			Input.Draw();
			
			Input.DebugDraw();
			Raylib.EndDrawing();

			if (Raylib.IsWindowResized())
			{
				Hierarchy.Transform.Size = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
			}
		}
	}
}