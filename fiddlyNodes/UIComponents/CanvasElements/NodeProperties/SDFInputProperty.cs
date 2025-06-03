using fiddlyNodes.Thistle;
using fiddlyNodes.Thistle.Library;
using Raylib_cs;

namespace fiddlyNodes.NodeElements;

public class SDFInputProperty : NodeProperty<TSDF>
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

	public override bool CanConnectTo(NodeProperty nodeProperty)
	{
		if (nodeProperty is NodeProperty<TSDFOperation>)
		{
			return true; // We can connect to any TSDFOperation property.
		}
		return base.CanConnectTo(nodeProperty);
	}

	public override TSDF GetValue()
	{
		var sdf = new TSDF();
		int propCount = 0;
		foreach (var property in InputPort.PropertiesFrom())
		{
			if (property is NodeProperty<TSDFOperation> opProp)
			{
				sdf.AddOperation(opProp.GetValue().Value);
				propCount++;
			}
		}

		if (propCount == 0)
		{
			sdf.AddOperation(new Constant(1));
		}
		
		return sdf;
	}
}