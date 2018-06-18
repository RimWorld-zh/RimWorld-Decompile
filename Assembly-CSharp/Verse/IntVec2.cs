using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEB RID: 3819
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x06005AAC RID: 23212 RVA: 0x002E615C File Offset: 0x002E455C
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x002E616D File Offset: 0x002E456D
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005AAE RID: 23214 RVA: 0x002E618C File Offset: 0x002E458C
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005AAF RID: 23215 RVA: 0x002E61B0 File Offset: 0x002E45B0
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x002E61D8 File Offset: 0x002E45D8
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005AB1 RID: 23217 RVA: 0x002E61F4 File Offset: 0x002E45F4
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x002E6210 File Offset: 0x002E4610
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x002E622C File Offset: 0x002E462C
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x002E6248 File Offset: 0x002E4648
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x002E6264 File Offset: 0x002E4664
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x002E6280 File Offset: 0x002E4680
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x002E629C File Offset: 0x002E469C
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x002E62D4 File Offset: 0x002E46D4
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x002E6300 File Offset: 0x002E4700
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x002E632C File Offset: 0x002E472C
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x002E6354 File Offset: 0x002E4754
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x002E6384 File Offset: 0x002E4784
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x002E63AC File Offset: 0x002E47AC
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

		// Token: 0x06005ABE RID: 23230 RVA: 0x002E640C File Offset: 0x002E480C
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x002E6450 File Offset: 0x002E4850
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

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005AC0 RID: 23232 RVA: 0x002E64B8 File Offset: 0x002E48B8
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x002E64DC File Offset: 0x002E48DC
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x002E6510 File Offset: 0x002E4910
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x002E6548 File Offset: 0x002E4948
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x002E6580 File Offset: 0x002E4980
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x002E65AC File Offset: 0x002E49AC
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005AC6 RID: 23238 RVA: 0x002E65D8 File Offset: 0x002E49D8
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x002E6600 File Offset: 0x002E4A00
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x002E6644 File Offset: 0x002E4A44
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x002E6688 File Offset: 0x002E4A88
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x002E66BC File Offset: 0x002E4ABC
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x002E66F8 File Offset: 0x002E4AF8
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x04003C8C RID: 15500
		public int x;

		// Token: 0x04003C8D RID: 15501
		public int z;
	}
}
