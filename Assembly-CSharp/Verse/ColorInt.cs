using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE4 RID: 3812
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x06005A4E RID: 23118 RVA: 0x002E4DDE File Offset: 0x002E31DE
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x002E4E01 File Offset: 0x002E3201
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x002E4E21 File Offset: 0x002E3221
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x002E4E58 File Offset: 0x002E3258
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x002E4EB0 File Offset: 0x002E32B0
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x002E4F08 File Offset: 0x002E3308
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x002E4F60 File Offset: 0x002E3360
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x002E4FA0 File Offset: 0x002E33A0
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x002E4FE8 File Offset: 0x002E33E8
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x002E5028 File Offset: 0x002E3428
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x002E5070 File Offset: 0x002E3470
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x002E50D4 File Offset: 0x002E34D4
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x002E5138 File Offset: 0x002E3538
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x002E516C File Offset: 0x002E356C
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x002E5190 File Offset: 0x002E3590
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x002E51E4 File Offset: 0x002E35E4
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

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x002E5240 File Offset: 0x002E3640
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

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06005A5F RID: 23135 RVA: 0x002E52B0 File Offset: 0x002E36B0
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

		// Token: 0x04003C72 RID: 15474
		public int r;

		// Token: 0x04003C73 RID: 15475
		public int g;

		// Token: 0x04003C74 RID: 15476
		public int b;

		// Token: 0x04003C75 RID: 15477
		public int a;
	}
}
