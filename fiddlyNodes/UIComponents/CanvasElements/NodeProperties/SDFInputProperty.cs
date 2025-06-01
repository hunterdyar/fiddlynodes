using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class SDFInputProperty : NodeProperty
{
	public SDFInputProperty(string propertyName, Node node) : base(propertyName, node)
	{
		AddAndSetPort(new Port(this, PortPosition.Input));
	}
	
	public override void Draw()
	{
		base.Draw();
		int hpad = 5; //get port size/2+pad.

		if (InputPort != null)
		{
			//.5 for port radius + .1 extra
			hpad = (int)(InputPort.Transform.Size.X * 0.6);
		}

		var w = _transform.WorldBounds;
		int fontSize = (int)(w.Height * 0.9f);
		int vpad = (int)Math.Floor(w.Height - fontSize) / 2; //vertically center

		Raylib.DrawText(propertyName, (int)w.X + hpad, (int)w.Y + vpad, fontSize, Color.Black);

	}

	public override TreeBaseObject GetValue(ThistleType wantedType)
	{
		if (wantedType != ThistleType.tsdf)
		{
			throw new Exception("sdfinput can only give sdf.");
		}
		var sdf = new TSDF();
		foreach (var property in InputPort.PropertiesFrom())
		{
			var value = property.GetValue(ThistleType.tsdfOp);
			if (value is TSDFOperation operation)
			{
				sdf.AddOperation(operation.Value);
			}
		}
		return sdf;
	}
}