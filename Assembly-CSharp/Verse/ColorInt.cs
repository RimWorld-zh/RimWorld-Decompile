using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE5 RID: 3813
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x06005A50 RID: 23120 RVA: 0x002E4D06 File Offset: 0x002E3106
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x002E4D29 File Offset: 0x002E3129
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x002E4D49 File Offset: 0x002E3149
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x002E4D80 File Offset: 0x002E3180
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x002E4DD8 File Offset: 0x002E31D8
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x002E4E30 File Offset: 0x002E3230
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x002E4E88 File Offset: 0x002E3288
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x002E4EC8 File Offset: 0x002E32C8
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x002E4F10 File Offset: 0x002E3310
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x002E4F50 File Offset: 0x002E3350
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x002E4F98 File Offset: 0x002E3398
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x002E4FFC File Offset: 0x002E33FC
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x002E5060 File Offset: 0x002E3460
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x002E5094 File Offset: 0x002E3494
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x002E50B8 File Offset: 0x002E34B8
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x002E510C File Offset: 0x002E350C
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

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06005A60 RID: 23136 RVA: 0x002E5168 File Offset: 0x002E3568
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06005A61 RID: 23137 RVA: 0x002E51D8 File Offset: 0x002E35D8
		public Color32 ToColor32
		{
			get
			{
				Color32 result = default(Color32);
				if (this.a > 255)
				{
					result.a = byte.MaxValue;
				}
				else
				{
					result.a = (byte)this.a;
				}
				if (this.r > 255)
				{
					result.r = byte.MaxValue;
				}
				else
				{
					result.r = (byte)this.r;
				}
				if (this.g > 255)
				{
					result.g = byte.MaxValue;
				}
				else
				{
					result.g = (byte)this.g;
				}
				if (this.b > 255)
				{
					result.b = byte.MaxValue;
				}
				else
				{
					result.b = (byte)this.b;
				}
				return result;
			}
		}

		// Token: 0x04003C73 RID: 15475
		public int r;

		// Token: 0x04003C74 RID: 15476
		public int g;

		// Token: 0x04003C75 RID: 15477
		public int b;

		// Token: 0x04003C76 RID: 15478
		public int a;
	}
}
