using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000747 RID: 1863
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x04001681 RID: 5761
		public QualityCategory min;

		// Token: 0x04001682 RID: 5762
		public QualityCategory max;

		// Token: 0x0600294B RID: 10571 RVA: 0x0015F4AC File Offset: 0x0015D8AC
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x0015F4C0 File Offset: 0x0015D8C0
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0015F4DC File Offset: 0x0015D8DC
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0015F50C File Offset: 0x0015D90C
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0015F548 File Offset: 0x0015D948
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0015F568 File Offset: 0x0015D968
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			QualityRange result = new QualityRange((QualityCategory)ParseHelper.FromString(array[0], typeof(QualityCategory)), (QualityCategory)ParseHelper.FromString(array[1], typeof(QualityCategory)));
			return result;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0015F5C8 File Offset: 0x0015D9C8
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0015F60C File Offset: 0x0015DA0C
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0015F640 File Offset: 0x0015DA40
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

		// Token: 0x06002954 RID: 10580 RVA: 0x0015F694 File Offset: 0x0015DA94
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
