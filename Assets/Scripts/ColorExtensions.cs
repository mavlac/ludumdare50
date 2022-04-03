using UnityEngine;

public static class ColorExtensions
{
	// Those are not extensions but are pretty handy
	public static Color ClearWhite => Color.white.WithAlpha(0);
	public static Color Orange => new Color(1.0f, 0.4f, 0f);
	
	
	
	public static Color WithAlpha(this Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}
	/// <summary>
	/// Sets alpha of color to complete transparency (0)
	/// </summary>
	/// <param name="color">Color</param>
	/// <returns>Altered Color</returns>
	public static Color Transparent(this Color color)
	{
		return color.WithAlpha(0f);
	}
	/// <summary>
	/// Sets alpha of color to half transparency (0.5)
	/// </summary>
	/// <param name="color">Color</param>
	/// <returns>Altered Color</returns>
	public static Color Translucent(this Color color)
	{
		return color.WithAlpha(0.5f);
	}
	/// <summary>
	/// Sets alpha of color to opaque (1)
	/// </summary>
	/// <param name="color">Color</param>
	/// <returns>Altered Color</returns>
	public static Color Opaque(this Color color)
	{
		return color.WithAlpha(1f);
	}
	/// <summary>
	/// Sets alpha of color to one half of current alpha (alpha * 0.5)
	/// </summary>
	/// <param name="color">Color</param>
	/// <returns>Altered Color</returns>
	public static Color HalveAlpha(this Color color)
	{
		return color.WithAlpha(color.a * 0.5f);
	}



	/// <summary>
	/// Decreases saturation while increasing value/brightness, by the same constant
	/// </summary>
	/// <param name="color">Color</param>
	/// <param name="effectAmount">Amount in range of 0..1</param>
	/// <returns>Altered Color</returns>
	public static Color Pastelize(this Color color, float effectAmount)
	{
		float a = color.a;
		Color.RGBToHSV(color, out float h, out float s, out float v);
		return Color.HSVToRGB(h, s - effectAmount, v + effectAmount).WithAlpha(a);
	}
}