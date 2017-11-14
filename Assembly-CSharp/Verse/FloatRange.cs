using System;
using UnityEngine;

namespace Verse
{
	public struct FloatRange : IEquatable<FloatRange>
	{
		public float min;

		public float max;

		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		public float Average
		{
			get
			{
				return (float)((this.min + this.max) / 2.0);
			}
		}

		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 9.9999997473787516E-06 && f <= this.max + 9.9999997473787516E-06;
		}

		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		public static FloatRange FromString(string s)
		{
			string[] array = s.Split('~');
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0]);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0]), Convert.ToSingle(array[1]));
		}

		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		public override int GetHashCode()
		{
			return Gen.HashCombineStruct(this.min.GetHashCode(), this.max);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FloatRange))
			{
				return false;
			}
			return this.Equals((FloatRange)obj);
		}

		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
