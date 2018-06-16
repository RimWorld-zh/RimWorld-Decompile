using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEA RID: 3818
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x06005A88 RID: 23176 RVA: 0x002E59A9 File Offset: 0x002E3DA9
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x002E59BC File Offset: 0x002E3DBC
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x002E59E0 File Offset: 0x002E3DE0
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x002E5A04 File Offset: 0x002E3E04
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x002E5A28 File Offset: 0x002E3E28
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x002E5A50 File Offset: 0x002E3E50
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x002E5A78 File Offset: 0x002E3E78
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x002E5AA0 File Offset: 0x002E3EA0
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x002E5AC8 File Offset: 0x002E3EC8
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x002E5AEC File Offset: 0x002E3EEC
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x002E5B14 File Offset: 0x002E3F14
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x002E5B3C File Offset: 0x002E3F3C
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x002E5B6C File Offset: 0x002E3F6C
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x002E5BA8 File Offset: 0x002E3FA8
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x002E5BD4 File Offset: 0x002E3FD4
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x002E5C10 File Offset: 0x002E4010
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x002E5C50 File Offset: 0x002E4050
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002E5C7C File Offset: 0x002E407C
		public static FloatRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			FloatRange result;
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0]);
				result = new FloatRange(num, num);
			}
			else
			{
				result = new FloatRange(Convert.ToSingle(array[0]), Convert.ToSingle(array[1]));
			}
			return result;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x002E5CDC File Offset: 0x002E40DC
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x002E5D14 File Offset: 0x002E4114
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x002E5D48 File Offset: 0x002E4148
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x002E5D7C File Offset: 0x002E417C
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04003C89 RID: 15497
		public float min;

		// Token: 0x04003C8A RID: 15498
		public float max;
	}
}
