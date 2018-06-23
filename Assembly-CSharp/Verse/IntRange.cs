using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEA RID: 3818
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x04003C9D RID: 15517
		public int min;

		// Token: 0x04003C9E RID: 15518
		public int max;

		// Token: 0x06005AC4 RID: 23236 RVA: 0x002E7EC1 File Offset: 0x002E62C1
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005AC5 RID: 23237 RVA: 0x002E7ED4 File Offset: 0x002E62D4
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005AC6 RID: 23238 RVA: 0x002E7EF0 File Offset: 0x002E62F0
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005AC7 RID: 23239 RVA: 0x002E7F0C File Offset: 0x002E630C
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005AC8 RID: 23240 RVA: 0x002E7F34 File Offset: 0x002E6334
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x002E7F5C File Offset: 0x002E635C
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x002E7F88 File Offset: 0x002E6388
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x002E7FB0 File Offset: 0x002E63B0
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x002E7FE4 File Offset: 0x002E63E4
		public static IntRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			IntRange result;
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0]);
				result = new IntRange(num, num);
			}
			else
			{
				result = new IntRange(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]));
			}
			return result;
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002E8044 File Offset: 0x002E6444
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x002E8088 File Offset: 0x002E6488
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x002E80B0 File Offset: 0x002E64B0
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06005AD0 RID: 23248 RVA: 0x002E80E4 File Offset: 0x002E64E4
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x002E8120 File Offset: 0x002E6520
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x002E8140 File Offset: 0x002E6540
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x002E8160 File Offset: 0x002E6560
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}
	}
}
