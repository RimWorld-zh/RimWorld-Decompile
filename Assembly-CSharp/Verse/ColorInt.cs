using System;
using UnityEngine;

namespace Verse
{
	public struct ColorInt : IEquatable<ColorInt>
	{
		public int r;

		public int g;

		public int b;

		public int a;

		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)((float)this.r / 255.0),
					g = (float)((float)this.g / 255.0),
					b = (float)((float)this.b / 255.0),
					a = (float)((float)this.a / 255.0)
				};
			}
		}

		public Color32 ToColor32
		{
			get
			{
				Color32 result = default(Color32);
				if (this.a > 255)
				{
					result.a = (byte)255;
				}
				else
				{
					result.a = (byte)this.a;
				}
				if (this.r > 255)
				{
					result.r = (byte)255;
				}
				else
				{
					result.r = (byte)this.r;
				}
				if (this.g > 255)
				{
					result.g = (byte)255;
				}
				else
				{
					result.g = (byte)this.g;
				}
				if (this.b > 255)
				{
					result.b = (byte)255;
				}
				else
				{
					result.b = (byte)this.b;
				}
				return result;
			}
		}

		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public ColorInt(Color32 col)
		{
			this.r = col.r;
			this.g = col.g;
			this.b = col.b;
			this.a = col.a;
		}

		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}
	}
}
