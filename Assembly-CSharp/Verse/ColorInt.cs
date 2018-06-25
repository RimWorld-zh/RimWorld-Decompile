using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE5 RID: 3813
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x04003C82 RID: 15490
		public int r;

		// Token: 0x04003C83 RID: 15491
		public int g;

		// Token: 0x04003C84 RID: 15492
		public int b;

		// Token: 0x04003C85 RID: 15493
		public int a;

		// Token: 0x06005A72 RID: 23154 RVA: 0x002E6D12 File Offset: 0x002E5112
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x002E6D35 File Offset: 0x002E5135
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x002E6D55 File Offset: 0x002E5155
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x002E6D8C File Offset: 0x002E518C
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x002E6DE4 File Offset: 0x002E51E4
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x002E6E3C File Offset: 0x002E523C
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x06005A78 RID: 23160 RVA: 0x002E6E94 File Offset: 0x002E5294
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x002E6ED4 File Offset: 0x002E52D4
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x002E6F1C File Offset: 0x002E531C
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x06005A7B RID: 23163 RVA: 0x002E6F5C File Offset: 0x002E535C
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06005A7C RID: 23164 RVA: 0x002E6FA4 File Offset: 0x002E53A4
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06005A7D RID: 23165 RVA: 0x002E7008 File Offset: 0x002E5408
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x002E706C File Offset: 0x002E546C
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x002E70A0 File Offset: 0x002E54A0
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x002E70C4 File Offset: 0x002E54C4
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x002E7118 File Offset: 0x002E5518
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

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06005A82 RID: 23170 RVA: 0x002E7174 File Offset: 0x002E5574
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

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x002E71E4 File Offset: 0x002E55E4
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
	}
}
