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
		Init();
		
		//test data
		LoadFile("autosave.json");

		while (!Raylib.WindowShouldClose())
		{
			//first do inputs and controls.
			Update();

			if (Raylib.IsWindowResized())
			{
				Hierarchy.Transform.Size = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
			}
		}
		File.WriteAllText("autosave.json", NodeFactory.SerializeProgram(), Encoding.UTF8);
	}

	public static void Update()
	{
		Input.Tick();
		Raylib.ClearBackground(UISettings.Active.BGColor);

		//draw.
		Raylib.BeginDrawing();
		Hierarchy.Draw();
		Input.Draw();
		NodeFinder.Draw();

		//Input.DebugDraw();
		Raylib.EndDrawing();
	}

	public static void Init()
	{
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(width, height, "fiddly widdly nodily wodily");
		int canvasWidth = (int)(width * .7f);
		int outputWidth = width - canvasWidth;
		GridCanvas = new GridCanvas(0, 0, canvasWidth, height);
		OutputContainer = new OutputContainer(canvasWidth, 0, outputWidth, outputWidth);
		Hierarchy.AddChild(GridCanvas);
		Hierarchy.AddChild(OutputContainer);
	}

	private static void LoadProgram(string data)
	{
		try
		{
			NodeFactory.DeserializeProgram(data, false);
			PrimaryOutputNode = (OutputNode)GridCanvas.GetAllNodes().Find(x => x.GetType() == typeof(OutputNode));
		}
		catch (Exception e)
		{
			Console.WriteLine($"Unable to program.");
			Console.WriteLine(e);
			LoadEmptyFile();
		}
	}
	private static void LoadFile(string path = "autosave.json")
	{
		try
		{
			var data = File.OpenRead(path);
			NodeFactory.DeserializeProgram(data, false);
			PrimaryOutputNode = (OutputNode)GridCanvas.GetAllNodes().Find(x => x.GetType() == typeof(OutputNode));
			data.Close();
		}
		catch (Exception e)
		{
			Console.WriteLine($"Unable to load file {path}");
			Console.WriteLine(e);
			LoadEmptyFile();
		}
		
	}

	private static void LoadEmptyFile()
	{
		PrimaryOutputNode = new OutputNode(300, 150, GridCanvas);
	}

	public static void Close()
	{
		Raylib.CloseWindow();
	}
}