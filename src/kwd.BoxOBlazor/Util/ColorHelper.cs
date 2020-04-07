using System;
using System.Globalization;

namespace kwd.BoxOBlazor.Util
{
    public static class ColorHelper
	{
		public static string RGBToHex(byte red, byte green, byte blue)
		{
			var txt = "#" + red.ToString("X2") + 
			          green.ToString("X2") + 
			          blue.ToString("X2");

			return txt;
		}

		/// <summary>Hex string to RGB</summary>
		public static (byte Red, byte Green, byte Blue) HexToRgb(string txt)
		{
			txt = txt.Trim().TrimStart('#');
		
			if (txt.Length != 6 && txt.Length != 8)
				throw new Exception("Hex must be either 6 or 8 characters");

			var val = BitConverter.GetBytes(
                uint.Parse(txt, NumberStyles.AllowHexSpecifier));

            return (val[0], val[1], val[2]);
		}

		/// <summary>
		/// Convert RGB space to HSL
		/// </summary>
		/// <remarks>
		/// Credits:
		/// https://www.programmingalgorithms.com/algorithm/rgb-to-hsl/
		/// </remarks>
		public static (int Hue, float Saturation, float Lightness) RGBToHSL(byte red, byte green, byte blue)
		{
			var r = (red / 255.0f);
			var g = (green / 255.0f);
			var b = (blue / 255.0f);
			
			var min = Math.Min(Math.Min(r, g), b);
			var max = Math.Max(Math.Max(r, g), b);

            var l = (max + min) / 2;

			var delta = max - min;

            
			if (delta.Like(0F))
            {
                return (0, 0, l);
            }

            var s = (l <= 0.5) ? (delta / (max + min)) : (delta / (2 - max - min));

			float hue;

			if (r.Like(max))
			{
				hue = ((g - b) / 6) / delta;
			}
			else if (g.Like(max))
			{
				hue = (1.0f / 3) + ((b - r) / 6) / delta;
			}
			else
			{
				hue = (2.0f / 3) + ((r - g) / 6) / delta;
			}

			if (hue < 0) hue += 1;
			if (hue > 1) hue -= 1;

			var h = (int)(hue * 360);
			

			return (h, s, l);
		}

		/// <summary>
		/// Convert HSL to RGB
		/// </summary>
		public static (byte R, byte G, byte B) HSLToRGB(
			int hue, float saturation, float lightness)
		{
			byte r, g, b;

            if (saturation.Like(0F))
            {
                var dull = (byte) (lightness * 255);
                return (dull, dull, dull);
			}
            
			var hueDeg = (float)hue / 360;

			var v2 = (lightness < 0.5) ? (lightness * (1 + saturation)) : 
				((lightness + saturation) - (lightness * saturation));
			var v1 = 2 * lightness - v2;

			r = (byte)(255 * HueToRGB(v1, v2, hueDeg + (1.0f / 3)));
			g = (byte)(255 * HueToRGB(v1, v2, hueDeg));
			b = (byte)(255 * HueToRGB(v1, v2, hueDeg - (1.0f / 3)));
		

			return (r, g, b);
		}

		private static float HueToRGB(float v1, float v2, float vH)
		{
			if (vH < 0) vH += 1;

			if (vH > 1) vH -= 1;

			if ((6 * vH) < 1)
				return (v1 + (v2 - v1) * 6 * vH);

			if ((2 * vH) < 1)
				return v2;

			if ((3 * vH) < 2)
				return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

			return v1;
		}
	}
}
