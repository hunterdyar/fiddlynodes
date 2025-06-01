using Raylib_cs;

namespace fiddlyNodes;

public class CommandSystem
{
	public List<Command> commandHistory = new List<Command>();
	public int maxNumberUndos = 512;
	public void AddAndExecute(Command command)
	{
		commandHistory.Add(command);
		command.Execute();
		//
		if (commandHistory.Count >= maxNumberUndos)
		{
			commandHistory.RemoveAt(0);
		}
	}

	private void Undo()
	{
		if (commandHistory.Count > 0)
		{
			var top = commandHistory[^1];
			commandHistory.RemoveAt(commandHistory.Count - 1);
			top.Undo();
			//stop undo-ing after we've undone all the clear commands.
		}
	}
	
	public void OnInput(InputEvent inputEvent)
	{
		if (inputEvent.Type == InputEventType.KeyPress)
		{
			if (inputEvent.KeyboardKey == KeyboardKey.Z)
			{
				Undo();
				inputEvent.Handle();
			}
		}
	}

	
}