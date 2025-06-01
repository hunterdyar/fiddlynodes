namespace fiddlyNodes;

public abstract class Command
{
	public CommandState State => _state;
	protected CommandState _state = CommandState.Uninitialized;

	public virtual void Execute()
	{
		if (_state == CommandState.Executed)
		{
			throw new Exception("Command already executed");
		}

		_state = CommandState.Executed;
	}

	public virtual void Undo()
	{
		if (_state != CommandState.Executed)
		{
			throw new Exception("Command not executed, cant be undone.");
		}

		_state = CommandState.Undone;
	}
}