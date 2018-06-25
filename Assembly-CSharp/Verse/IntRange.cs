using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEC RID: 3820
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x04003C9D RID: 15517
		public int min;

		// Token: 0x04003C9E RID: 15518
		public int max;

		// Token: 0x06005AC7 RID: 23239 RVA: 0x002E7FE1 File Offset: 0x002E63E1
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005AC8 RID: 23240 RVA: 0x002E7FF4 File Offset: 0x002E63F4
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x002E8010 File Offset: 0x002E6410
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x002E802C File Offset: 0x002E642C
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005ACB RID: 23243 RVA: 0x002E8054 File Offset: 0x002E6454
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005ACC RID: 23244 RVA: 0x002E807C File Offset: 0x002E647C
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x002E80A8 File Offset: 0x002E64A8
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x002E80D0 File Offset: 0x002E64D0
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x002E8104 File Offset: 0x002E6504
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

		// Token: 0x06005AD0 RID: 23248 RVA: 0x002E8164 File Offset: 0x002E6564
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x002E81A8 File Offset: 0x002E65A8
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x002E81D0 File Offset: 0x002E65D0
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x002E8204 File Offset: 0x002E6604
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06005AD4 RID: 23252 RVA: 0x002E8240 File Offset: 0x002E6640
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x002E8260 File Offset: 0x002E6660
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x002E8280 File Offset: 0x002E6680
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}
	}
}
