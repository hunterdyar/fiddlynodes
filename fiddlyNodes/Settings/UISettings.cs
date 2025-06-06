using Raylib_cs;

namespace fiddlyNodes;

public class UISettings
{
	public static UISettings Active = new UISettings();
	public float PropertyBaseHeight = 12;
	public float PropertyPadding;

	public Color BGColor = ColorUtils.LightOneCandle;
	public Color GridDotColor = ColorUtils.CandyNougat;
	public Color NodeBGColor = ColorUtils.Warmth;
	public Color NodeBorderColor = ColorUtils.WarmD;
	public Color TextColor = Color.Black;
	public Color WireColor = ColorUtils.PuppySnuggle;
	public Color WireHoverColor = Color.Blue;
	public Color WireMarkForDeleteColor = Color.Red;
	public Color PortColor = Color.Black;
	public Color PortHoverColor = Color.Blue;
		
		
	public float NodeBorderRoundness = 0.2f;
	public float NodeBorderThickness = 1f;
	
	//save
	public Font Font => Raylib.GetFontDefault();
}