using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEE RID: 3822
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x04003CA7 RID: 15527
		public int x;

		// Token: 0x04003CA8 RID: 15528
		public int z;

		// Token: 0x06005AD7 RID: 23255 RVA: 0x002E84D0 File Offset: 0x002E68D0
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x002E84E1 File Offset: 0x002E68E1
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002E8500 File Offset: 0x002E6900
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x002E8524 File Offset: 0x002E6924
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x002E854C File Offset: 0x002E694C
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002E8568 File Offset: 0x002E6968
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x002E8584 File Offset: 0x002E6984
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x002E85A0 File Offset: 0x002E69A0
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002E85BC File Offset: 0x002E69BC
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x002E85D8 File Offset: 0x002E69D8
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x002E85F4 File Offset: 0x002E69F4
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005AE2 RID: 23266 RVA: 0x002E8610 File Offset: 0x002E6A10
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005AE3 RID: 23267 RVA: 0x002E8648 File Offset: 0x002E6A48
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x002E8674 File Offset: 0x002E6A74
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x002E86A0 File Offset: 0x002E6AA0
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x002E86C8 File Offset: 0x002E6AC8
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x002E86F8 File Offset: 0x002E6AF8
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x002E8720 File Offset: 0x002E6B20
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

		// Token: 0x06005AE9 RID: 23273 RVA: 0x002E8780 File Offset: 0x002E6B80
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x002E87C4 File Offset: 0x002E6BC4
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
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x002E882C File Offset: 0x002E6C2C
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x002E8850 File Offset: 0x002E6C50
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002E8884 File Offset: 0x002E6C84
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x002E88BC File Offset: 0x002E6CBC
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x002E88F4 File Offset: 0x002E6CF4
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x002E8920 File Offset: 0x002E6D20
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x002E894C File Offset: 0x002E6D4C
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x002E8974 File Offset: 0x002E6D74
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x002E89B8 File Offset: 0x002E6DB8
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x002E89FC File Offset: 0x002E6DFC
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x002E8A30 File Offset: 0x002E6E30
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002E8A6C File Offset: 0x002E6E6C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}
	}
}
