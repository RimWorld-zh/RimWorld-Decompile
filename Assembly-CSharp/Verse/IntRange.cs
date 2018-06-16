using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEB RID: 3819
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x06005A9E RID: 23198 RVA: 0x002E5DB5 File Offset: 0x002E41B5
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x002E5DC8 File Offset: 0x002E41C8
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x002E5DE4 File Offset: 0x002E41E4
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x002E5E00 File Offset: 0x002E4200
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x002E5E28 File Offset: 0x002E4228
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005AA3 RID: 23203 RVA: 0x002E5E50 File Offset: 0x002E4250
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x002E5E7C File Offset: 0x002E427C
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x002E5EA4 File Offset: 0x002E42A4
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x002E5ED8 File Offset: 0x002E42D8
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

		// Token: 0x06005AA7 RID: 23207 RVA: 0x002E5F38 File Offset: 0x002E4338
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x002E5F7C File Offset: 0x002E437C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x002E5FA4 File Offset: 0x002E43A4
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x002E5FD8 File Offset: 0x002E43D8
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x002E6014 File Offset: 0x002E4414
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x002E6034 File Offset: 0x002E4434
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x002E6054 File Offset: 0x002E4454
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x04003C8B RID: 15499
		public int min;

		// Token: 0x04003C8C RID: 15500
		public int max;
	}
}
