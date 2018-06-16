using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074B RID: 1867
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x06002950 RID: 10576 RVA: 0x0015F240 File Offset: 0x0015D640
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x0015F254 File Offset: 0x0015D654
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0015F270 File Offset: 0x0015D670
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0015F2A0 File Offset: 0x0015D6A0
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0015F2DC File Offset: 0x0015D6DC
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0015F2FC File Offset: 0x0015D6FC
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			QualityRange result = new QualityRange((QualityCategory)ParseHelper.FromString(array[0], typeof(QualityCategory)), (QualityCategory)ParseHelper.FromString(array[1], typeof(QualityCategory)));
			return result;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0015F35C File Offset: 0x0015D75C
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0015F3A0 File Offset: 0x0015D7A0
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0015F3D4 File Offset: 0x0015D7D4
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

		// Token: 0x06002959 RID: 10585 RVA: 0x0015F428 File Offset: 0x0015D828
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04001683 RID: 5763
		public QualityCategory min;

		// Token: 0x04001684 RID: 5764
		public QualityCategory max;
	}
}
