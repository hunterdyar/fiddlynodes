using System.ComponentModel;
using System.Numerics;
using System.Text;
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
		LoadFile();

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
		File.WriteAllText("autosave.txt", NodeFactory.SerializeProgram(), Encoding.UTF8);
	}

	private static void LoadFile()
	{
		var data = File.OpenRead("autosave.txt");
		NodeFactory.DeserializeProgram(data, false);
		PrimaryOutputNode = (OutputNode)GridCanvas.GetAllNodes().Find(x=>x.GetType() == typeof(OutputNode));
		data.Close();
	}

	private static void LoadEmptyFile()
	{
		PrimaryOutputNode = new OutputNode(300, 150, GridCanvas);
	}
}