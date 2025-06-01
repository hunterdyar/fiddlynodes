using System.Numerics;
using fiddlyNodes.NodeElements;
using fiddlyNodes.UIDraw;
using Raylib_cs;

namespace fiddlyNodes;

public class Wire(Port from, Port to, WireManager manager) : IHoverable, IEquatable<Wire>
{
	private readonly WireManager _manager = manager;
	public Port FromPort = from;
	public Port ToPort = to;
	private bool _markedForDelete = false;
	private bool _hovering = false;
	public float thickness = 2;
	public float HoverPixelThreshold = 6;
	//type, etc.

	public void Draw()
	{
		var c = _hovering ? Color.Blue : Color.Black;
		if (_markedForDelete)
		{
			c = Color.Red;
		}
		UIDrawHelpers.DrawWire(FromPort.Transform.WorldPivotPosition, ToPort.Transform.WorldPivotPosition, thickness, c);
	}

	public void SetHovering(bool hovering)
	{
		this._hovering = hovering;
	}

	public bool OverlapsPoint(Vector2 pos)
	{
		var d = ElementUtility.DistanceToLine(FromPort.Transform.WorldPivotPosition, ToPort.Transform.WorldPivotPosition, pos);
		return d < HoverPixelThreshold+thickness;
	}

	public void SetMarkedForDelete(bool markedForDelete)
	{
		_markedForDelete = markedForDelete;
	}
	public void OnLoseHover()
	{
		_hovering = false;
	}

	public void OnGainHover()
	{
		_hovering = true;
	}

	public void Remove()
	{
		_manager.RemoveWire(this);
	}

	public bool Equals(Wire? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		return _manager.Equals(other._manager) && FromPort.Equals(other.FromPort) && ToPort.Equals(other.ToPort) && thickness.Equals(other.thickness) && HoverPixelThreshold.Equals(other.HoverPixelThreshold);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((Wire)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_manager, FromPort, ToPort, thickness, HoverPixelThreshold);
	}

	public static bool operator ==(Wire? left, Wire? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(Wire? left, Wire? right)
	{
		return !Equals(left, right);
	}
}