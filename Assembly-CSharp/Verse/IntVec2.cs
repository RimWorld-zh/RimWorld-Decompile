using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEC RID: 3820
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x06005AAE RID: 23214 RVA: 0x002E6084 File Offset: 0x002E4484
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x002E6095 File Offset: 0x002E4495
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x002E60B4 File Offset: 0x002E44B4
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005AB1 RID: 23217 RVA: 0x002E60D8 File Offset: 0x002E44D8
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x002E6100 File Offset: 0x002E4500
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x002E611C File Offset: 0x002E451C
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x002E6138 File Offset: 0x002E4538
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x002E6154 File Offset: 0x002E4554
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x002E6170 File Offset: 0x002E4570
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x002E618C File Offset: 0x002E458C
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x002E61A8 File Offset: 0x002E45A8
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x002E61C4 File Offset: 0x002E45C4
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005ABA RID: 23226 RVA: 0x002E61FC File Offset: 0x002E45FC
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005ABB RID: 23227 RVA: 0x002E6228 File Offset: 0x002E4628
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x002E6254 File Offset: 0x002E4654
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x002E627C File Offset: 0x002E467C
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x002E62AC File Offset: 0x002E46AC
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x002E62D4 File Offset: 0x002E46D4
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

		// Token: 0x06005AC0 RID: 23232 RVA: 0x002E6334 File Offset: 0x002E4734
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x002E6378 File Offset: 0x002E4778
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

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x002E63E0 File Offset: 0x002E47E0
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x002E6404 File Offset: 0x002E4804
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x002E6438 File Offset: 0x002E4838
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x002E6470 File Offset: 0x002E4870
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x002E64A8 File Offset: 0x002E48A8
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x002E64D4 File Offset: 0x002E48D4
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005AC8 RID: 23240 RVA: 0x002E6500 File Offset: 0x002E4900
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x002E6528 File Offset: 0x002E4928
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x002E656C File Offset: 0x002E496C
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x002E65B0 File Offset: 0x002E49B0
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x002E65E4 File Offset: 0x002E49E4
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002E6620 File Offset: 0x002E4A20
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x04003C8D RID: 15501
		public int x;

		// Token: 0x04003C8E RID: 15502
		public int z;
	}
}
