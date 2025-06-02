using Raylib_cs;

namespace fiddlyNodes;

public class UISettings
{
	public static UISettings Active = new UISettings();
	
	public float PropertyBaseHeight = 12;
	public float PropertyPadding;
	
	//load
	
	//save
	public Font Font => Raylib.GetFontDefault();
}