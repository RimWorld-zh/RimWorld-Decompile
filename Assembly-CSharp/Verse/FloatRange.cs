using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEC RID: 3820
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x04003CA3 RID: 15523
		public float min;

		// Token: 0x04003CA4 RID: 15524
		public float max;

		// Token: 0x06005AB1 RID: 23217 RVA: 0x002E7DF5 File Offset: 0x002E61F5
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x002E7E08 File Offset: 0x002E6208
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x002E7E2C File Offset: 0x002E622C
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x002E7E50 File Offset: 0x002E6250
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x002E7E74 File Offset: 0x002E6274
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x002E7E9C File Offset: 0x002E629C
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x002E7EC4 File Offset: 0x002E62C4
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x002E7EEC File Offset: 0x002E62EC
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x002E7F14 File Offset: 0x002E6314
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x002E7F38 File Offset: 0x002E6338
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x002E7F60 File Offset: 0x002E6360
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x002E7F88 File Offset: 0x002E6388
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x002E7FB8 File Offset: 0x002E63B8
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x002E7FF4 File Offset: 0x002E63F4
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x002E8020 File Offset: 0x002E6420
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x002E805C File Offset: 0x002E645C
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x002E809C File Offset: 0x002E649C
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x002E80C8 File Offset: 0x002E64C8
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

		// Token: 0x06005AC3 RID: 23235 RVA: 0x002E8128 File Offset: 0x002E6528
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x002E8160 File Offset: 0x002E6560
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x002E8194 File Offset: 0x002E6594
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x002E81C8 File Offset: 0x002E65C8
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
