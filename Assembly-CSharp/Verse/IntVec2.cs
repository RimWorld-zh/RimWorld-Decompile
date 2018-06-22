using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEB RID: 3819
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x06005AD4 RID: 23252 RVA: 0x002E8190 File Offset: 0x002E6590
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x002E81A1 File Offset: 0x002E65A1
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x002E81C0 File Offset: 0x002E65C0
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x002E81E4 File Offset: 0x002E65E4
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005AD8 RID: 23256 RVA: 0x002E820C File Offset: 0x002E660C
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002E8228 File Offset: 0x002E6628
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x002E8244 File Offset: 0x002E6644
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x002E8260 File Offset: 0x002E6660
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002E827C File Offset: 0x002E667C
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x002E8298 File Offset: 0x002E6698
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x002E82B4 File Offset: 0x002E66B4
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002E82D0 File Offset: 0x002E66D0
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x002E8308 File Offset: 0x002E6708
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x002E8334 File Offset: 0x002E6734
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x002E8360 File Offset: 0x002E6760
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x002E8388 File Offset: 0x002E6788
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x002E83B8 File Offset: 0x002E67B8
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x002E83E0 File Offset: 0x002E67E0
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

		// Token: 0x06005AE6 RID: 23270 RVA: 0x002E8440 File Offset: 0x002E6840
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x002E8484 File Offset: 0x002E6884
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

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005AE8 RID: 23272 RVA: 0x002E84EC File Offset: 0x002E68EC
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x002E8510 File Offset: 0x002E6910
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x002E8544 File Offset: 0x002E6944
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x002E857C File Offset: 0x002E697C
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x002E85B4 File Offset: 0x002E69B4
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002E85E0 File Offset: 0x002E69E0
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005AEE RID: 23278 RVA: 0x002E860C File Offset: 0x002E6A0C
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x002E8634 File Offset: 0x002E6A34
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x002E8678 File Offset: 0x002E6A78
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x002E86BC File Offset: 0x002E6ABC
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x002E86F0 File Offset: 0x002E6AF0
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x002E872C File Offset: 0x002E6B2C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x04003C9F RID: 15519
		public int x;

		// Token: 0x04003CA0 RID: 15520
		public int z;
	}
}
