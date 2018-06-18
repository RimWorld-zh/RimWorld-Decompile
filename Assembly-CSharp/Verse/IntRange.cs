using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEA RID: 3818
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x06005A9C RID: 23196 RVA: 0x002E5E8D File Offset: 0x002E428D
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005A9D RID: 23197 RVA: 0x002E5EA0 File Offset: 0x002E42A0
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005A9E RID: 23198 RVA: 0x002E5EBC File Offset: 0x002E42BC
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x002E5ED8 File Offset: 0x002E42D8
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x002E5F00 File Offset: 0x002E4300
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x002E5F28 File Offset: 0x002E4328
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x002E5F54 File Offset: 0x002E4354
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x002E5F7C File Offset: 0x002E437C
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x002E5FB0 File Offset: 0x002E43B0
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

		// Token: 0x06005AA5 RID: 23205 RVA: 0x002E6010 File Offset: 0x002E4410
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x002E6054 File Offset: 0x002E4454
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x002E607C File Offset: 0x002E447C
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x002E60B0 File Offset: 0x002E44B0
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x002E60EC File Offset: 0x002E44EC
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x002E610C File Offset: 0x002E450C
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x002E612C File Offset: 0x002E452C
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x04003C8A RID: 15498
		public int min;

		// Token: 0x04003C8B RID: 15499
		public int max;
	}
}
