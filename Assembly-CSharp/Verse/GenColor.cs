using System.Globalization;
using UnityEngine;

namespace Verse
{
	public static class GenColor
	{
		public static Color SaturationChanged(this Color col, float change)
		{
			float r = col.r;
			float g = col.g;
			float b = col.b;
			float num = Mathf.Sqrt((float)(r * r * 0.29899999499320984 + g * g * 0.58700001239776611 + b * b * 0.11400000005960464));
			r = num + (r - num) * change;
			g = num + (g - num) * change;
			b = num + (b - num) * change;
			return new Color(r, g, b);
		}

		public static bool IndistinguishableFrom(this Color colA, Color colB)
		{
			Color color = colA - colB;
			return Mathf.Abs(color.r) + Mathf.Abs(color.g) + Mathf.Abs(color.b) + Mathf.Abs(color.a) < 0.0010000000474974513;
		}

		public static Color RandomColorOpaque()
		{
			return new Color(Rand.Value, Rand.Value, Rand.Value, 1f);
		}

		public static Color FromBytes(int r, int g, int b, int a = 255)
		{
			return new Color
			{
				r = (float)((float)r / 255.0),
				g = (float)((float)g / 255.0),
				b = (float)((float)b / 255.0),
				a = (float)((float)a / 255.0)
			};
		}

		public static Color FromHex(string hex)
		{
			if (hex.StartsWith("#"))
			{
				hex = hex.Substring(1);
			}
			Color result;
			if (hex.Length != 6 && hex.Length != 8)
			{
				Log.Error(hex + " is not a valid hex color.");
				result = Color.white;
			}
			else
			{
				int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
				int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
				int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
				int a = 255;
				if (hex.Length == 8)
				{
					a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
				}
				result = GenColor.FromBytes(r, g, b, a);
			}
			return result;
		}
	}
}
