using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEB RID: 3819
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x04003C9B RID: 15515
		public float min;

		// Token: 0x04003C9C RID: 15516
		public float max;

		// Token: 0x06005AB1 RID: 23217 RVA: 0x002E7BD5 File Offset: 0x002E5FD5
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x002E7BE8 File Offset: 0x002E5FE8
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x002E7C0C File Offset: 0x002E600C
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x002E7C30 File Offset: 0x002E6030
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x002E7C54 File Offset: 0x002E6054
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x002E7C7C File Offset: 0x002E607C
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x002E7CA4 File Offset: 0x002E60A4
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x002E7CCC File Offset: 0x002E60CC
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x002E7CF4 File Offset: 0x002E60F4
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x002E7D18 File Offset: 0x002E6118
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x002E7D40 File Offset: 0x002E6140
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x002E7D68 File Offset: 0x002E6168
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x002E7D98 File Offset: 0x002E6198
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x002E7DD4 File Offset: 0x002E61D4
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x002E7E00 File Offset: 0x002E6200
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x002E7E3C File Offset: 0x002E623C
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x002E7E7C File Offset: 0x002E627C
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x002E7EA8 File Offset: 0x002E62A8
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

		// Token: 0x06005AC3 RID: 23235 RVA: 0x002E7F08 File Offset: 0x002E6308
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x002E7F40 File Offset: 0x002E6340
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x002E7F74 File Offset: 0x002E6374
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x002E7FA8 File Offset: 0x002E63A8
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
