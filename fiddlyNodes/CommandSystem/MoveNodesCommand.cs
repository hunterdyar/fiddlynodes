using System.Numerics;

namespace fiddlyNodes;

public class MoveNodesCommand : Command
{
	private Node _node;
	private Vector2 originalPosition;
	private Vector2 newPosition;
	public MoveNodesCommand(Node node, Vector2 originalLocalPosition, Vector2 newLocalPosition)
	{
		_node = node;
		this.originalPosition = originalLocalPosition;
		this.newPosition = newLocalPosition;
	}
	public override void Execute()
	{
		_node.Transform.LocalPosition = newPosition;
		base.Execute();
	}

	public override void Undo()
	{
		_node.Transform.LocalPosition = originalPosition;
		base.Undo();
	}
}