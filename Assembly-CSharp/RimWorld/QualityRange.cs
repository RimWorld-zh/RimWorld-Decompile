using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000749 RID: 1865
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x04001685 RID: 5765
		public QualityCategory min;

		// Token: 0x04001686 RID: 5766
		public QualityCategory max;

		// Token: 0x0600294E RID: 10574 RVA: 0x0015F85C File Offset: 0x0015DC5C
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x0015F870 File Offset: 0x0015DC70
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0015F88C File Offset: 0x0015DC8C
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0015F8BC File Offset: 0x0015DCBC
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0015F8F8 File Offset: 0x0015DCF8
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0015F918 File Offset: 0x0015DD18
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			QualityRange result = new QualityRange((QualityCategory)ParseHelper.FromString(array[0], typeof(QualityCategory)), (QualityCategory)ParseHelper.FromString(array[1], typeof(QualityCategory)));
			return result;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0015F978 File Offset: 0x0015DD78
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0015F9BC File Offset: 0x0015DDBC
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0015F9F0 File Offset: 0x0015DDF0
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

		// Token: 0x06002957 RID: 10583 RVA: 0x0015FA44 File Offset: 0x0015DE44
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
