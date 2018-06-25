using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000749 RID: 1865
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x04001681 RID: 5761
		public QualityCategory min;

		// Token: 0x04001682 RID: 5762
		public QualityCategory max;

		// Token: 0x0600294F RID: 10575 RVA: 0x0015F5FC File Offset: 0x0015D9FC
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x0015F610 File Offset: 0x0015DA10
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0015F62C File Offset: 0x0015DA2C
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0015F65C File Offset: 0x0015DA5C
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0015F698 File Offset: 0x0015DA98
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0015F6B8 File Offset: 0x0015DAB8
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			QualityRange result = new QualityRange((QualityCategory)ParseHelper.FromString(array[0], typeof(QualityCategory)), (QualityCategory)ParseHelper.FromString(array[1], typeof(QualityCategory)));
			return result;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0015F718 File Offset: 0x0015DB18
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0015F75C File Offset: 0x0015DB5C
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0015F790 File Offset: 0x0015DB90
		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is QualityRange))
			{
				result = false;
			}
			else
			{
				QualityRange qualityRange = (QualityRange)obj;
				result = (qualityRange.min == this.min && qualityRange.max == this.max);
			}
			return result;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0015F7E4 File Offset: 0x0015DBE4
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
