using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE9 RID: 3817
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x06005A86 RID: 23174 RVA: 0x002E5A81 File Offset: 0x002E3E81
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x002E5A94 File Offset: 0x002E3E94
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06005A88 RID: 23176 RVA: 0x002E5AB8 File Offset: 0x002E3EB8
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x002E5ADC File Offset: 0x002E3EDC
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x002E5B00 File Offset: 0x002E3F00
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x002E5B28 File Offset: 0x002E3F28
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x002E5B50 File Offset: 0x002E3F50
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x002E5B78 File Offset: 0x002E3F78
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x002E5BA0 File Offset: 0x002E3FA0
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x002E5BC4 File Offset: 0x002E3FC4
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x002E5BEC File Offset: 0x002E3FEC
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x002E5C14 File Offset: 0x002E4014
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x002E5C44 File Offset: 0x002E4044
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x002E5C80 File Offset: 0x002E4080
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x002E5CAC File Offset: 0x002E40AC
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x002E5CE8 File Offset: 0x002E40E8
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x002E5D28 File Offset: 0x002E4128
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x002E5D54 File Offset: 0x002E4154
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

		// Token: 0x06005A98 RID: 23192 RVA: 0x002E5DB4 File Offset: 0x002E41B4
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002E5DEC File Offset: 0x002E41EC
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x002E5E20 File Offset: 0x002E4220
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x002E5E54 File Offset: 0x002E4254
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04003C88 RID: 15496
		public float min;

		// Token: 0x04003C89 RID: 15497
		public float max;
	}
}
