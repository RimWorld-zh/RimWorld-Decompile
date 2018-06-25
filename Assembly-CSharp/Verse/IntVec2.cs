using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EED RID: 3821
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x04003C9F RID: 15519
		public int x;

		// Token: 0x04003CA0 RID: 15520
		public int z;

		// Token: 0x06005AD7 RID: 23255 RVA: 0x002E82B0 File Offset: 0x002E66B0
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x002E82C1 File Offset: 0x002E66C1
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002E82E0 File Offset: 0x002E66E0
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x002E8304 File Offset: 0x002E6704
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x002E832C File Offset: 0x002E672C
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002E8348 File Offset: 0x002E6748
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x002E8364 File Offset: 0x002E6764
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x002E8380 File Offset: 0x002E6780
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002E839C File Offset: 0x002E679C
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x002E83B8 File Offset: 0x002E67B8
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x002E83D4 File Offset: 0x002E67D4
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005AE2 RID: 23266 RVA: 0x002E83F0 File Offset: 0x002E67F0
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005AE3 RID: 23267 RVA: 0x002E8428 File Offset: 0x002E6828
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x002E8454 File Offset: 0x002E6854
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x002E8480 File Offset: 0x002E6880
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x002E84A8 File Offset: 0x002E68A8
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x002E84D8 File Offset: 0x002E68D8
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x002E8500 File Offset: 0x002E6900
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x002E8560 File Offset: 0x002E6960
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x002E85A4 File Offset: 0x002E69A4
		public static IntVec2 FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			int newX = Convert.ToInt32(array[0]);
			int newZ = Convert.ToInt32(array[1]);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x002E860C File Offset: 0x002E6A0C
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x002E8630 File Offset: 0x002E6A30
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002E8664 File Offset: 0x002E6A64
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x002E869C File Offset: 0x002E6A9C
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x002E86D4 File Offset: 0x002E6AD4
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x002E8700 File Offset: 0x002E6B00
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x002E872C File Offset: 0x002E6B2C
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x002E8754 File Offset: 0x002E6B54
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x002E8798 File Offset: 0x002E6B98
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x002E87DC File Offset: 0x002E6BDC
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x002E8810 File Offset: 0x002E6C10
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002E884C File Offset: 0x002E6C4C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}
	}
}
