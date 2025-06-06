using System.Globalization;
using Raylib_cs;

public static class ColorUtils
{
	public static Color FromHex(string hex)
	{
		hex = hex.TrimStart('#');
		int r, g, b = 0;
		r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
		g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
		b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
		return new Color(r, g, b);
	}

	public static Color WarmD = FromHex("DB9E86");
	public static Color Warmth = FromHex("F5C7AB");
	public static Color LightOneCandle = FromHex("FEF7F8");
	public static Color CandyNougat = FromHex("C9B486");
	public static Color PuppySnuggle = FromHex("9E734B");
}
