using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EED RID: 3821
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x04003CA5 RID: 15525
		public int min;

		// Token: 0x04003CA6 RID: 15526
		public int max;

		// Token: 0x06005AC7 RID: 23239 RVA: 0x002E8201 File Offset: 0x002E6601
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005AC8 RID: 23240 RVA: 0x002E8214 File Offset: 0x002E6614
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x002E8230 File Offset: 0x002E6630
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x002E824C File Offset: 0x002E664C
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005ACB RID: 23243 RVA: 0x002E8274 File Offset: 0x002E6674
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005ACC RID: 23244 RVA: 0x002E829C File Offset: 0x002E669C
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x002E82C8 File Offset: 0x002E66C8
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x002E82F0 File Offset: 0x002E66F0
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x002E8324 File Offset: 0x002E6724
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

		// Token: 0x06005AD0 RID: 23248 RVA: 0x002E8384 File Offset: 0x002E6784
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x002E83C8 File Offset: 0x002E67C8
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x002E83F0 File Offset: 0x002E67F0
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x002E8424 File Offset: 0x002E6824
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06005AD4 RID: 23252 RVA: 0x002E8460 File Offset: 0x002E6860
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x002E8480 File Offset: 0x002E6880
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x002E84A0 File Offset: 0x002E68A0
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}
	}
}
